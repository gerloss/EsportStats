using EsportStats.Server.Services;
using EsportStats.Shared.DTO;
using EsportStats.Shared.Enums;
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
        [Route("{m:int?}")]
        public async Task<ActionResult<ICollection<TopListEntryDTO>>> Get(int? m)
        {
            if (!m.HasValue)
            {
                return BadRequest("You must select a value for the metric.");
            }

            var metric = (Metric)m.Value;

            if (metric == Metric.PleaseSelect)
            {
                return BadRequest("You must select a value for the metric.");
            }

            try
            {
                var topList = await _topListService.GetByMetricAsync("", metric);
                return Ok(topList);
            }
            catch
            {
                //TODO: Logging/Error message
                return BadRequest(); // TODO: Proper response depending on what the error is
            }
        }



    }
}
