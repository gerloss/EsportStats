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
    public class ProfileController : ControllerBase
    {
        private ISteamService _steamService { get; }

        public ProfileController(ISteamService steamService)
        {
            _steamService = steamService;
        }


        /// <summary>
        /// Gets the list of Steam friends of the currently authenticated user.
        /// </summary>        
        [Route("friends")]
        [HttpGet]
        public async Task<ActionResult<ICollection<SteamFriendDTO>>> GetFriends()
        {

            try
            {
                var friends = await _steamService.GetFriendsAsync();
                return Ok(friends.OrderByDescending(f => f.HoursPlayed));

            }
            catch
            {
                //TODO: Logging/Error message
                return BadRequest(); // TODO: Proper response depending on what the error is
            }
        }
    }
}
