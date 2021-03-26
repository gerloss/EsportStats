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
    // TODO: Authentication    
    [Route("api/[controller]")]
    [ApiController]    
    public class FriendsController : ControllerBase
    {
        private ISteamFriendService _steamFriendService { get; }

        public FriendsController(ISteamFriendService steamFriendService)
        {
            _steamFriendService = steamFriendService;
        }

        /// <summary>
        /// Gets the list of Steam friends of the currently authenticated user.
        /// </summary>        
        [HttpGet]
        public async Task<ActionResult<ICollection<SteamFriend>>> Get() {

            try
            {
                var friends = await _steamFriendService.GetFriendsAsync();
                return Ok(friends);

            } catch {
                //TODO: Logging/Error message
                return BadRequest(); // TODO: Proper response depending on what the error is
            }
        }
    }
}
