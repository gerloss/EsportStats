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
        private IUserService _userService { get; }

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Gets the steam profile for the currently authenticated user
        /// </summary>        
        [HttpGet]        
        public async Task<ActionResult<SteamUserDTO>> GetProfile()
        {
            var id = Convert.ToUInt64(HttpContext.User.FindFirst(JwtClaimTypes.Id)?.Value);
            var profile = await _userService.GetUserAsync(id);
            return Ok(profile.ToDTO());
        }        
    }
}
