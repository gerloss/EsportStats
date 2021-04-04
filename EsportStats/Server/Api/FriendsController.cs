using EsportStats.Server.Services;
using EsportStats.Shared.DTO;
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
    public class FriendsController : ControllerBase
    {
        private readonly ISteamService _steamService;

        public FriendsController(ISteamService steamService)
        {
            _steamService = steamService;
        }

        /// <summary>
        /// Gets the list of Steam friends of the currently authenticated user.
        /// </summary>                
        [HttpGet]
        public async Task<ActionResult<ICollection<SteamUserDTO>>> GetFriends()
        {
            var friends = await _steamService.GetFriendsAsync();
            return Ok(friends.OrderByDescending(f => f.HoursPlayed));
        }
    }
}
