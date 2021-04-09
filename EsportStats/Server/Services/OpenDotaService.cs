﻿using EsportStats.Server.Common;
using EsportStats.Server.Data;
using EsportStats.Server.Data.Entities;
using EsportStats.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EsportStats.Shared.Common;
using Newtonsoft.Json;

namespace EsportStats.Server.Services
{
    public interface IOpenDotaService
    {
        public Task<IEnumerable<HeroStatDTO>> GetHeroStatsAsync(string userId);
        public Task<IEnumerable<HeroStatDTO>> GetHeroStatsAsync(ulong steamId);

        public Task<IEnumerable<TopListEntryExtDTO>> GetTopListEntriesAsync(string userId, Metric metric);
        public Task<IEnumerable<TopListEntryExtDTO>> GetTopListEntriesAsync(ulong steamId, Metric metric);
    }

    public class OpenDotaService : IOpenDotaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _httpClientFactory;        

        public OpenDotaService(
            IUnitOfWork unitOfWork,
            IHttpClientFactory httpClientFactory)
        {
            _unitOfWork = unitOfWork;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Gets the hero statistics of the user with the given userid.
        /// </summary>       
        public async Task<IEnumerable<HeroStatDTO>> GetHeroStatsAsync(string userId)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);
            return await GetHeroStatsAsync(user.SteamId);
        }

        /// <summary>
        /// Gets the hero statistics of the user with the given steamid64.
        /// </summary>    
        public async Task<IEnumerable<HeroStatDTO>> GetHeroStatsAsync(ulong steamId)
        {
            var heroStatsUrl = $"https://api.opendota.com/api/players/{steamId.ToSteam32()}/heroes";

            var httpClient = _httpClientFactory.CreateClient();
            var heroStatsResponse = await httpClient.GetAsync(heroStatsUrl);
            var response = await heroStatsResponse.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<List<HeroStatDTO>>(response);

            return parsedResponse;            
        }

        public async Task<IEnumerable<TopListEntryExtDTO>> GetTopListEntriesAsync(string userId, Metric metric)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);
            return await GetTopListEntriesAsync(user.SteamId, metric);
        }

        public async Task<IEnumerable<TopListEntryExtDTO>> GetTopListEntriesAsync(ulong steamId, Metric metric)
        {            
            var entriesUrl = $"https://api.opendota.com/api/players/{steamId.ToSteam32()}/matches?sort={metric.GetShortName()}&limit=10";

            var httpClient = _httpClientFactory.CreateClient();
            var heroStatsResponse = await httpClient.GetAsync(entriesUrl);
            var response = await heroStatsResponse.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<List<TopListEntryExtDTO>>(response);

            return parsedResponse;
            throw new NotImplementedException();
        }
    }
}
