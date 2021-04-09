using EsportStats.Server.Services;
using EsportStats.Shared.DTO;
using EsportStats.Shared.Enums;
using Microsoft.AspNetCore.Http;
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
        private ITopListService _topListService { get; }

        public ListsController(ITopListService topListService)
        {
            _topListService = topListService;
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
                throw new ArgumentException("m", "There was no metric selected.");
            }

            if (!Enum.IsDefined(typeof(Metric), m))
            {
                throw new ArgumentOutOfRangeException("m", $"There is no Metric with the id of {m}");
            }

            var metric = (Metric) m;

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var topList = await _topListService.GetByMetricAsync(currentUserId, metric);
            return Ok(topList);
        }
    }
}
