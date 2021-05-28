using EsportStats.Server.Common;
using EsportStats.Server.Data.Entities;
using EsportStats.Server.Services;
using EsportStats.Shared.DTO;
using EsportStats.Shared.Enums;
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
            /// Gets the list of top hero spammers within the friends of the currently authenticated user.
            /// </summary>                
            [HttpGet("spammers")]
            public async Task<ActionResult<ICollection<TopListEntryDTO>>> GetSpammers()
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var currentUser = await _userManager.FindByIdAsync(currentUserId);

                var entries = await _heroStatService.GetSpammersAsync(currentUser.SteamId);
                return Ok(entries.OrderByDescending(r => r.Value));
            }

            /// <summary>
            /// Gets the list of top players by hero within the friends of the currently authenticated user.
            /// </summary>                
            [HttpGet("heroes/{heroId}")]
            public async Task<ActionResult<ICollection<TopListEntryDTO>>> GetSpammers(int heroId)
            {
                if (heroId == 0)
                {
                    throw new ApiArgumentException("heroId", "There was no hero selected.");                    
                }

                if (!Enum.IsDefined(typeof(Hero), heroId)) {
                    throw new ApiArgumentOutOfRangeException("heroId", $"There is no Hero with the id of {heroId}");
                }

                var hero = (Hero) heroId;

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var currentUser = await _userManager.FindByIdAsync(currentUserId);

                var entries = await _heroStatService.GetTopByHeroAsync(currentUser.SteamId, hero);
                return Ok(entries.OrderByDescending(r => r.Value));
            }
        }
    }
}
