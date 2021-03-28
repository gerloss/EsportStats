using EsportStats.Server.Data.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EsportStats.Server.Common
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var claims = await base.GenerateClaimsAsync(user);            

            claims.AddClaim(new Claim(JwtClaimTypes.Name, user.Name));
            claims.AddClaim(new Claim(JwtClaimTypes.Picture, user.AvatarFull));
            claims.AddClaim(new Claim(JwtClaimTypes.Profile, user.ProfileUrl));
            claims.AddClaim(new Claim(JwtClaimTypes.Id, user.SteamId.ToString()));

            return claims;
        }
    }
}
