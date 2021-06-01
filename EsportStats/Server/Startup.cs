using AspNet.Security.OpenId.Steam;
using EsportStats.Client;
using EsportStats.Server.Common;
using EsportStats.Server.Data;
using EsportStats.Server.Data.Entities;
using EsportStats.Server.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Hellang.Middleware.ProblemDetails;
using System;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace EsportStats.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(options => { 
                    options.SignIn.RequireConfirmedAccount = false; 
                    options.ClaimsIdentity.UserIdClaimType = System.Security.Claims.ClaimTypes.NameIdentifier; 
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>();

            services.Configure<CookiePolicyOptions>(options => {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.OnAppendCookie = cookieContext =>
                    CheckSameSiteCookies(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext =>
                    CheckSameSiteCookies(cookieContext.Context, cookieContext.CookieOptions);            
            });

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt()
                .AddSteam();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (context, ex) => 
                {
                    return context.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment(); // include exception details if its dev env
                };
                options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
                options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
                options.MapToStatusCode<TooManyRequestsException>(StatusCodes.Status429TooManyRequests);
                options.MapToStatusCode<ApiArgumentException>(StatusCodes.Status400BadRequest);
                options.MapToStatusCode<ApiArgumentNullException>(StatusCodes.Status400BadRequest);
                options.MapToStatusCode<ApiArgumentOutOfRangeException>(StatusCodes.Status400BadRequest);


            });

            var steamOptions = new SteamOptions();
            Configuration.Bind(SteamOptions.Steam, steamOptions);
            services.AddSingleton(steamOptions);

            var openDotaOptions = new OpenDotaOptions();
            Configuration.Bind(OpenDotaOptions.OpenDota, openDotaOptions);
            services.AddSingleton(openDotaOptions);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISteamService, SteamService>();
            services.AddScoped<ITopListService, TopListService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IHeroStatService, HeroStatService>();
            services.AddScoped<IOpenDotaService, OpenDotaService>();

            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseProblemDetails(); // add the error handling middleware to the request processing pipeline
            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();                         

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }

        private void CheckSameSiteCookies(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                

                if (UserAgentNotSupported(userAgent))
                {                    
                    options.SameSite = SameSiteMode.Unspecified;
                }
            }
        }

        private bool UserAgentNotSupported(string userAgent)
        {
            // Cover all iOS-based browsers here. This includes:
            //   - Safari on iOS 12 for iPhone, iPod Touch, iPad
            //   - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
            //   - Chrome on iOS 12 for iPhone, iPod Touch, iPad
            // All of which are broken by SameSite=None, because they use the
            // iOS networking stack.
            // Notes from Thinktecture:
            // Regarding https://caniuse.com/#search=samesite iOS versions lower
            // than 12 are not supporting SameSite at all. Starting with version 13
            // unknown values are NOT treated as strict anymore. Therefore we only
            // need to check version 12.
            if (userAgent.Contains("CPU iPhone OS 12")
               || userAgent.Contains("iPad; CPU OS 12"))
            {
                return true;
            }

            // Cover Mac OS X based browsers that use the Mac OS networking stack.
            // This includes:
            //   - Safari on Mac OS X.
            // This does not include:
            //   - Chrome on Mac OS X
            // because they do not use the Mac OS networking stack.
            // Notes from Thinktecture:
            // Regarding https://caniuse.com/#search=samesite MacOS X versions lower
            // than 10.14 are not supporting SameSite at all. Starting with version
            // 10.15 unknown values are NOT treated as strict anymore. Therefore we
            // only need to check version 10.14.
            if (userAgent.Contains("Safari")
               && userAgent.Contains("Macintosh; Intel Mac OS X 10_14")
               && userAgent.Contains("Version/"))
            {
                return true;
            }

            // Cover Chrome 50-69, because some versions are broken by SameSite=None
            // and none in this range require it.
            // Note: this covers some pre-Chromium Edge versions,
            // but pre-Chromium Edge does not require SameSite=None.
            // Notes from Thinktecture:
            // We can not validate this assumption, but we trust Microsofts
            // evaluation. And overall not sending a SameSite value equals to the same
            // behavior as SameSite=None for these old versions anyways.
            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
            {
                return true;
            }

            return false;
        }
    }
}
