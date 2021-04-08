﻿using EsportStats.Server.Data;
using EsportStats.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EsportStats.Server.Services
{
    public interface IOpenDotaService
    {
        public Task<Dictionary<Hero, int>> GetHeroStatsAsync(string userId);
        public Task<Dictionary<Hero, int>> GetHeroStatsAsync(ulong steamId);
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
        /// Serves the hero statistics of the user with the given userid.
        /// </summary>       
        public async Task<Dictionary<Hero, int>> GetHeroStatsAsync(string userId)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);
            return await GetHeroStatsAsync(user.SteamId);
        }

        /// <summary>
        /// Serves the hero statistics of the user with the given steamid64.
        /// </summary>    
        public async Task<Dictionary<Hero, int>> GetHeroStatsAsync(ulong steamId)
        {
            // TODO:
            // Check database if we have up-to-date hero stats            

            // If not, call OpenDota api (through its service) for fresh stats

            throw new NotImplementedException();
        }
    }
}
