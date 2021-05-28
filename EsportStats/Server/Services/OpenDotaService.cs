using EsportStats.Server.Common;
using EsportStats.Server.Data;
using EsportStats.Server.Data.Entities;
using EsportStats.Shared.Enums;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EsportStats.Shared.Common;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace EsportStats.Server.Services
{
    public interface IOpenDotaService
    {
        public Task<IEnumerable<HeroStatDTO>> GetHeroStatsAsync(ulong steamId);
        public Task<IEnumerable<TopListEntryExtDTO>> GetTopListEntriesAsync(ulong steamId, Metric metric);
    }

    public class OpenDotaService : IOpenDotaService
    {        
        private readonly IHttpClientFactory _httpClientFactory;        
        private readonly string _apiKey;

        public OpenDotaService(
            IHttpClientFactory httpClientFactory,
            OpenDotaOptions openDotaOptions)
        {            
            _httpClientFactory = httpClientFactory;            
            _apiKey = openDotaOptions.Key;
        }

        /// <summary>
        /// Gets the hero statistics of the user with the given steamid64.
        /// </summary>    
        public async Task<IEnumerable<HeroStatDTO>> GetHeroStatsAsync(ulong steamId)
        {            
            var heroStatsUrl = $"https://api.opendota.com/api/players/{steamId.ToSteam32()}/heroes?api_key=" + _apiKey;

            var httpClient = _httpClientFactory.CreateClient();
            var heroStatsResponse = await httpClient.GetAsync(heroStatsUrl);
            var response = await heroStatsResponse.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<List<HeroStatDTO>>(response);

            return parsedResponse;            
        }


        public async Task<IEnumerable<TopListEntryExtDTO>> GetTopListEntriesAsync(ulong steamId, Metric metric)
        {            
            var entriesUrl = $"https://api.opendota.com/api/players/{steamId.ToSteam32()}/matches?sort={metric.GetShortName()}&limit=10&api_key=" + _apiKey;

            var httpClient = _httpClientFactory.CreateClient();
            var heroStatsResponse = await httpClient.GetAsync(entriesUrl);            

            if (heroStatsResponse.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                throw new TooManyRequestsException("OpenDota API is overloaded.");
            }

            var response = await heroStatsResponse.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<List<TopListEntryExtDTO>>(response);

            return parsedResponse;
        }
    }
}
