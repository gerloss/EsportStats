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

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();                         

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile(env.IsDevelopment() ? "index.html" : "wwwroot/index.html"); // after a -release deployment the client wwwroot is nested inside the server wwwroot
            });
        }
    }
}
