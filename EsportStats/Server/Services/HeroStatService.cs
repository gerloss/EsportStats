using EsportStats.Server.Data;
using EsportStats.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EsportStats.Server.Services
{
    public interface IHeroStatService
    {
        public Task<Dictionary<Hero, int>> GetHeroStatsAsync(string userId);
        public Task<Dictionary<Hero, int>> GetHeroStatsAsync(ulong steamId);
    }

    public class HeroStatService : IHeroStatService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _httpClientFactory;

        public HeroStatService(
            IUnitOfWork unitOfWork,
            IHttpClientFactory httpClientFactory)
        {
            _unitOfWork = unitOfWork;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Dictionary<Hero, int>> GetHeroStatsAsync(string userId)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);
            return await GetHeroStatsAsync(user.SteamId);
        }

        public async Task<Dictionary<Hero, int>> GetHeroStatsAsync(ulong steamId)
        {
            // https://api.opendota.com/api/players/{steamid32}/heroes
            throw new NotImplementedException();
        }
    }
}
