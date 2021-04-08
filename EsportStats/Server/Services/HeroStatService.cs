using EsportStats.Server.Data;
using EsportStats.Server.Data.Entities;
using EsportStats.Shared.DTO;
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

        public Task<IEnumerable<TopListEntryDTO>> GetSpammersAsync(string userId, int take = 25);
        public Task<IEnumerable<TopListEntryDTO>> GetSpammersAsync(ulong steamId, int take = 25);

        public Task<IEnumerable<TopListEntryDTO>> GetTopByHeroAsync(string userId, Hero hero);
        public Task<IEnumerable<TopListEntryDTO>> GetTopByHeroAsync(ulong steamId, Hero hero);
    }

    public class HeroStatService : IHeroStatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOpenDotaService _openDotaService;
        private readonly ISteamService _steamService;

        public HeroStatService(
            IUnitOfWork unitOfWork,
            IOpenDotaService openDotaService,
            ISteamService steamService)
        {
            _unitOfWork = unitOfWork;
            _openDotaService = openDotaService;
            _steamService = steamService;
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
            ExternalUser extPlayer = null;
            if (player == null)
            {
                extPlayer = await _unitOfWork.ExternalUsers.GetAsync(steamId);
                if (extPlayer == null)
                {
                    // If there is any valid data about this user, then the user should already exist, because they have been persisted during loading the front page.
                    // So this probably means that no statistics are available.
                    return new List<KeyValuePair<Hero, int>>();
                }
            }

            // From here we don't care if its an ApplicationUser or an ExternalUser, we only need their HeroStats regardless
            var stats = await _unitOfWork.HeroStats.GetHeroStatsBySteamIdAsync(steamId);            
            var timestamp = player != null ? player.HeroStatsTimestamp : extPlayer.HeroStatsTimestamp;            

            if (!timestamp.HasValue || timestamp < DateTime.Now.AddHours(-24))
            {
                // Stats are not up to date, so we refresh them from opendota api
                var updatedStats = await _openDotaService.GetHeroStatsAsync(steamId);

                if (!timestamp.HasValue && !stats.Any())
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

        public async Task<IEnumerable<TopListEntryDTO>> GetSpammersAsync(string userId, int take = 25)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);
            return await GetSpammersAsync(user.SteamId, take);
        }

        public async Task<IEnumerable<TopListEntryDTO>> GetSpammersAsync(ulong steamId, int take = 25)
        {
            // Playtime should not be refreshed, as it will not be required and it would take a lot of extra time...
            // This way this can be done in a single HTTP GET towards Steam API
            var players = await _steamService.GetFriendsAsync(steamId, includePlayer: true, includePlaytime: false);
            
            var stats = new List<TopListEntryDTO>(); // key: playerId, value: List of HeroStats
            // Get the hero stats for every player
            foreach(var player in players)
            {
                var heroStatsKVP = await GetHeroStatsAsync(player.SteamId);
                var heroStatsDTO = heroStatsKVP.Select(stat => new TopListEntryDTO() { 
                    Friend = player,
                    Hero = stat.Key,
                    Value = stat.Value
                });

                stats.AddRange(heroStatsDTO);
            }

            return stats.OrderByDescending(s => s.Value).Take(take);
        }

        public async Task<IEnumerable<TopListEntryDTO>> GetTopByHeroAsync(string userId, Hero hero)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);
            return await GetTopByHeroAsync(user.SteamId, hero);
        }

        public async Task<IEnumerable<TopListEntryDTO>> GetTopByHeroAsync(ulong steamId, Hero hero)
        {
            throw new NotImplementedException();
        }
    }
}
