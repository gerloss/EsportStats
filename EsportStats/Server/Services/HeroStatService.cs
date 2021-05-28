using EsportStats.Server.Common;
using EsportStats.Server.Data;
using EsportStats.Server.Data.Entities;
using EsportStats.Shared.DTO;
using EsportStats.Shared.Enums;
using Microsoft.Extensions.Configuration;
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
        public Task<List<HeroStatsResult>> GetHeroStatsAsync(string userId);
        public Task<List<HeroStatsResult>> GetHeroStatsAsync(ulong steamId);

        public Task<IEnumerable<TopListEntryDTO>> GetSpammersAsync(string userId, int take = 25);
        public Task<IEnumerable<TopListEntryDTO>> GetSpammersAsync(ulong steamId, int take = 25);

        public Task<IEnumerable<TopListEntryDTO>> GetTopByHeroAsync(string userId, Hero hero, int take = 10);
        public Task<IEnumerable<TopListEntryDTO>> GetTopByHeroAsync(ulong steamId, Hero hero, int take = 10);
    }

    public class HeroStatService : IHeroStatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOpenDotaService _openDotaService;
        private readonly ISteamService _steamService;
        private readonly SteamOptions _steamOptions;
        private readonly OpenDotaOptions _openDotaOptions;

        public HeroStatService(
            IUnitOfWork unitOfWork,
            IOpenDotaService openDotaService,
            ISteamService steamService,
            SteamOptions steamOptions,
            OpenDotaOptions openDotaOptions)
        {
            _unitOfWork = unitOfWork;
            _openDotaService = openDotaService;
            _steamService = steamService;
            _steamOptions = steamOptions;
            _openDotaOptions = openDotaOptions;
        }

        /// <summary>
        /// Gets all the hero statistics of the user with the given userid.
        /// </summary>    
        public async Task<List<HeroStatsResult>> GetHeroStatsAsync(string userId)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);
            return await GetHeroStatsAsync(user.SteamId);
        }

        /// <summary>
        /// Gets all the hero statistics of the user with the given steamId.
        /// </summary>    
        public async Task<List<HeroStatsResult>> GetHeroStatsAsync(ulong steamId)
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
                    return new List<HeroStatsResult>();
                }
            }

            // From here we don't care if its an ApplicationUser or an ExternalUser, we only need their HeroStats regardless
            var stats = await _unitOfWork.HeroStats.GetHeroStatsBySteamIdAsync(steamId);            
            var timestamp = player != null ? player.HeroStatsTimestamp : extPlayer.HeroStatsTimestamp;            

            if (!timestamp.HasValue || timestamp < DateTime.Now.AddHours(-24))
            {
                // Stats are not up to date, so we refresh them from opendota api
                var updatedStats = await _openDotaService.GetHeroStatsAsync(steamId);
                var createdStats = new List<HeroStat>();
                if (!timestamp.HasValue && !stats.Any())
                {
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
                        var stat = stats.FirstOrDefault(stat => stat.Hero == dto.Hero);
                        if (stat != null)
                        {
                            stat.Games = dto.Games;
                        }
                        else
                        {
                            // there was no statistics for this specific hero yet, so this must be added
                            createdStats.Add(new HeroStat(dto, steamId));
                        }
                    }
                    if (createdStats.Any())
                    {
                        await _unitOfWork.HeroStats.AddRangeAsync(createdStats);
                    }
                }

                if (player != null)
                {
                    player.HeroStatsTimestamp = DateTime.Now;
                }
                else
                {
                    extPlayer.HeroStatsTimestamp = DateTime.Now;
                }
                
                _unitOfWork.SaveChanges();

                return updatedStats.Select(stat => new HeroStatsResult 
                { 
                    SteamId = steamId,
                    Hero = stat.Hero,
                    Value = stat.Games
                }).ToList();
                 
            }
            else
            {
                // stats are up to date, return from db:                
                return stats.Select(stat => new HeroStatsResult 
                {
                    SteamId = steamId,
                    Hero = stat.Hero,
                    Value = stat.Games
                }).ToList();

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
            
            var stats = new List<TopListEntryDTO>();
            // Get the hero stats for every player                        
            var tasks = players.Select(p => GetHeroStatsAsync(p.SteamId)); // complete tasks in batches
            var numberOfBatches = (int)Math.Ceiling((double)tasks.Count() / _openDotaOptions.BatchSize);
            var heroStats = new List<TopListEntryDTO>();

            for (int i = 0; i < numberOfBatches; i++)
            {
                var batch = tasks.Skip(i * _openDotaOptions.BatchSize).Take(_openDotaOptions.BatchSize);
                var results = (await Task.WhenAll(batch));
                foreach(var playerResult in results)
                {
                    if (playerResult.Any())
                    {
                        var player = players.Single(p => p.SteamId == playerResult.First().SteamId);                        

                        heroStats.AddRange(playerResult.Select(stat => new TopListEntryDTO
                        {
                            Friend = player,
                            Hero = stat.Hero,
                            Value = stat.Value,
                            MatchId = null
                        }));
                    }
                }                
            }

            var ordered = stats.OrderByDescending(s => s.Value);
            var topValues = ordered.Take(take);
            if(!topValues.Any(v => v.Friend.IsCurrentPlayer) && ordered.Any(v => v.Friend.IsCurrentPlayer))
            {                
                topValues = topValues.Append(ordered.First(v => v.Friend.IsCurrentPlayer));
            }
            return topValues.OrderByDescending(s => s.Value);
        }

        public async Task<IEnumerable<TopListEntryDTO>> GetTopByHeroAsync(string userId, Hero hero, int take = 10)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);
            return await GetTopByHeroAsync(user.SteamId, hero, take);
        }

        public async Task<IEnumerable<TopListEntryDTO>> GetTopByHeroAsync(ulong steamId, Hero hero, int take = 10)
        {
            // Playtime should not be refreshed, as it will not be required and it would take a lot of extra time...
            // This way this can be done in a single HTTP GET towards Steam API
            var players = await _steamService.GetFriendsAsync(steamId, includePlayer: true, includePlaytime: false);
            // Get the hero stats for every player
            var tasks = players.Select(p => GetHeroStatsAsync(p.SteamId));

            var numberOfBatches = (int)Math.Ceiling((double)tasks.Count() / _openDotaOptions.BatchSize);
            var heroStats = new List<TopListEntryDTO>();

            for (int i = 0; i < numberOfBatches; i++)
            {
                var batch = tasks.Skip(i * _openDotaOptions.BatchSize).Take(_openDotaOptions.BatchSize);
                var results = (await Task.WhenAll(batch));
                foreach (var playerResult in results)
                {
                    if (playerResult.Any())
                    {
                        var player = players.Single(p => p.SteamId == playerResult.First().SteamId);                        
                        heroStats.AddRange(playerResult.Select(stat => new TopListEntryDTO
                        {
                            Friend = player,
                            Hero = stat.Hero,
                            Value = stat.Value,
                            MatchId = null
                        }));
                    }
                }
            }

            var filtered = heroStats.Where(stat => stat.Hero == hero && stat.Value > 0);
            var ordered = heroStats.OrderByDescending(s => s.Value);            
            var topValues = ordered.Take(take);
            if (!topValues.Any(v => v.Friend.IsCurrentPlayer))
            {
                var currentPlayerStat = heroStats.SingleOrDefault(stat => stat.Friend.IsCurrentPlayer && stat.Hero == hero);
                if (currentPlayerStat != null)
                {
                    topValues = topValues.Append(currentPlayerStat);
                }                
            }
            return topValues.OrderByDescending(s => s.Value);
        }
    }
}
