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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FriendsController : ControllerBase
    {
        private readonly ISteamService _steamService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FriendsController(ISteamService steamService, UserManager<ApplicationUser> userManager)
        {
            _steamService = steamService;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets the list of Steam friends of the currently authenticated user.
        /// </summary>                
        [HttpGet]
        public async Task<ActionResult<ICollection<SteamUserDTO>>> GetFriends()
        {            
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var friends = await _steamService.GetFriendsAsync(currentUserId);
            return Ok(friends.OrderByDescending(f => f.Playtime));
        }
    }
}
