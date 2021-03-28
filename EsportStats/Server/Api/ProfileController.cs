using EsportStats.Server.Services;
using EsportStats.Shared.DTO;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private ISteamService _steamService { get; }
        private IUserService _userService { get; }

        public ProfileController(ISteamService steamService, IUserService userService)
        {
            _steamService = steamService;
            _userService = userService;
        }


        /// <summary>
        /// Gets the steam profile for the currently authenticated user
        /// </summary>        
        [HttpGet]
        [Route("{steamId?:uint}")]
        [Authorize]
        public async Task<ActionResult<SteamFriendDTO>> GetProfile(ulong? steamId)
        {            
            if (!steamId.HasValue)
            {
                steamId = Convert.ToUInt64(HttpContext.User.FindFirst(JwtClaimTypes.Id)?.Value);
            }

            var profile = await _userService.GetUserAsync(steamId.Value);
            return Ok(profile);
        }


        /// <summary>
        /// Gets the list of Steam friends of the currently authenticated user.
        /// </summary>        
        [Route("friends")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ICollection<SteamFriendDTO>>> GetFriends()
        {
            var friends = await _steamService.GetFriendsAsync();
            return Ok(friends.OrderByDescending(f => f.HoursPlayed));
        }
    }
}
