using EsportStats.Server.Common;
using EsportStats.Server.Data.Entities;
using EsportStats.Server.Services;
using EsportStats.Shared.DTO;
using EsportStats.Shared.Enums;
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
    // TODO: Authentication    
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly ITopListService _topListService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ListsController(
            ITopListService topListService, 
            UserManager<ApplicationUser> userManager)
        {
            _topListService = topListService;
            _userManager = userManager;
        }        

        /// <summary>
        /// Gets the list of values for the selected metric from the Steam friends of the currently authenticated user.
        /// </summary>        
        [HttpGet]
        [Route("{m}")]
        public async Task<ActionResult<ICollection<TopListEntryDTO>>> Get(int m)
        {
            if (m == 0)
            {
                throw new ApiArgumentException("m", "There was no metric selected.");
            }

            if (!Enum.IsDefined(typeof(Metric), m))
            {
                throw new ApiArgumentOutOfRangeException("m", $"There is no Metric with the id of {m}");
            }

            var metric = (Metric) m;
            
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var currentUser = await _userManager.FindByIdAsync(currentUserId);

            var topList = await _topListService.GetByMetricForUser(currentUser.SteamId, metric);
            return Ok(topList);
        }
    }
}
