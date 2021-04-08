using EsportStats.Server.Data;
using EsportStats.Server.Data.Entities;
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
        // Since Dictionary is not sorted, we use a List of KeyValuePairs instead
        public Task<List<KeyValuePair<Hero, int>>> GetHeroStatsAsync(string userId);
        public Task<List<KeyValuePair<Hero, int>>> GetHeroStatsAsync(ulong steamId);
    }

    public class HeroStatService : IHeroStatService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IOpenDotaService _openDotaService;

        public HeroStatService(
            IUnitOfWork unitOfWork,
            IOpenDotaService openDotaService)
        {
            _unitOfWork = unitOfWork;
            _openDotaService = openDotaService;
        }

        /// <summary>
        /// Gets all the hero statistics of the user with the given userid.
        /// </summary>    
        public async Task<List<KeyValuePair<Hero, int>>> GetHeroStatsAsync(string userId)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);
            return await GetHeroStatsAsync(user.SteamId);
        }

        /// <summary>
        /// Gets all the hero statistics of the user with the given steamId.
        /// </summary>    
        public async Task<List<KeyValuePair<Hero, int>>> GetHeroStatsAsync(ulong steamId)
        {
            var player = await _unitOfWork.Users.GetUserBySteamIdAsync(steamId);
            if (player == null)
            {
                // The pages listing the hero stats should only be displayed for logged in users
                // so there is not point in looking for ExternalUsers with the same steamId;
                throw new ArgumentException($"No user can be found with the SteamId64 {steamId}");
            }

            var stats = await _unitOfWork.HeroStats.GetHeroStatsBySteamIdAsync(steamId);            

            if (!player.HeroStatsTimestamp.HasValue || player.HeroStatsTimestamp < DateTime.Now.AddHours(-24))
            {
                // Stats are not up to date, so we refresh them from opendota api
                var updatedStats = await _openDotaService.GetHeroStatsAsync(steamId);

                if (!player.HeroStatsTimestamp.HasValue && !stats.Any())
                {
                    var createdStats = new List<HeroStat>();
                    // No hero stats recorded yet, new entities should be created
                    foreach(var dto in updatedStats)
                    {
                        createdStats.Add(new HeroStat(dto, steamId));
                    }
                    await _unitOfWork.HeroStats.AddRangeAsync(createdStats);
                }
                else
                {
                    // Hero stats already exist, only update the already existing entities
                    foreach (var dto in updatedStats)
                    {
                        stats.First(stat => stat.Hero == dto.Hero).Games = dto.Games;
                    }                    
                }

                player.HeroStatsTimestamp = DateTime.Now;
                _unitOfWork.SaveChanges();

                return updatedStats.Select(stat => new KeyValuePair<Hero, int>(stat.Hero, stat.Games)).ToList();
            }
            else
            {
                // stats are up to date, return from db:
                return stats.Select(stat => new KeyValuePair<Hero, int>(stat.Hero, stat.Games)).ToList();
            }
        }
    }
}
