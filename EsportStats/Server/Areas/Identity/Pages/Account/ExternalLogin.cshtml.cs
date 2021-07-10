using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using EsportStats.Server.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using EsportStats.Server.Common;
using Microsoft.Extensions.Configuration;
using EsportStats.Server.Services;
using EsportStats.Server.Data;

namespace EsportStats.Server.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly ISteamService _steamService;
        private readonly IUnitOfWork _unitOfWork;

        public ExternalLoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender,
            ISteamService steamService,
            IUnitOfWork unitOfWork
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _steamService = steamService;
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new {ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor : true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then create one automatically.
                // In case of Steam OpenID Provider, the returned Claimed ID will contain the user's 64 bit SteamID.                
                // More info at: https://steamcommunity.com/dev
                                
                ulong steamId = Convert.ToUInt64(info.ProviderKey.Split('/', StringSplitOptions.RemoveEmptyEntries).Last());

                // The Claimed ID format is: https://steamcommunity.com/openid/id/<steamid>

                var steamProfile = await _steamService.GetSteamProfileExternalAsync(steamId);

                var user = new ApplicationUser 
                { 
                    UserName = steamId.ToString(),
                    Email = steamId.ToString(),
                    SteamId = steamId,
                    Name = steamProfile.Name,
                    ProfileUrl = steamProfile.ProfileUrl,
                    Avatar = steamProfile.Avatar,
                    AvatarFull = steamProfile.AvatarFull,
                    Timestamp = DateTime.Now
                };

                var createResult = await _userManager.CreateAsync(user);
                if (createResult.Succeeded)
                {
                    // check if the user's profile already existed as an ExternalUser entity
                    var externalUser = await _unitOfWork.ExternalUsers.GetAsync(user.SteamId);
                    if (externalUser != null)
                    {
                        // get the external user's TopListEntries and move them to the newly created ApplicationUser
                        var entries = await _unitOfWork.TopListEntries.GetTopEntriesForSteamIdAsync(user.SteamId);
                        foreach(var entry in entries.Where(e => e.User == null && e.ExternalUserId.HasValue && e.ExternalUserId.Value == externalUser.SteamId))
                        {
                            entry.ExternalUserId = null;
                            entry.UserId = user.Id;
                        }

                        _unitOfWork.ExternalUsers.Remove(externalUser);
                        _unitOfWork.SaveChanges();
                    }

                    user.Playtime = (await _steamService.GetSteamPlaytimeMinutesAsync(user.SteamId)).Value;
                    user.PlaytimeTimestamp = DateTime.Now;
                    await _userManager.UpdateAsync(user);                    

                    var loginResult = await _userManager.AddLoginAsync(user, info);
                    if (loginResult.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        ErrorMessage = "Error creating user for this Steam account.";
                        return RedirectToPage("./Login");
                    }
                }
                else
                {
                    ErrorMessage = "Error creating user for this Steam account.";
                    return RedirectToPage("./Login");
                }

            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }
    }
}
