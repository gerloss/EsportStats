using EsportStats.Server.Data.Entities;
using EsportStats.Server.Services;
using EsportStats.Shared.DTO;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EsportStats.Server.Api
{
    namespace EsportStats.Server.Api
    {
        [Route("api")]
        [ApiController]
        [Authorize]
        public class HeroesController : ControllerBase
        {
            private readonly IHeroStatService _heroStatService;
            private readonly UserManager<ApplicationUser> _userManager;

            public HeroesController(IHeroStatService heroStatService, UserManager<ApplicationUser> userManager)
            {
                _heroStatService = heroStatService;
                _userManager = userManager;
            }

            /// <summary>
            /// Gets the list of Steam friends of the currently authenticated user.
            /// </summary>                
            [HttpGet("spammers")]
            public async Task<ActionResult<ICollection<TopListEntryDTO>>> GetSpammers()
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var entries = await _heroStatService.GetSpammersAsync(currentUserId);
                return Ok(entries.OrderByDescending(r => r.Value));
            }
        }
    }
}
