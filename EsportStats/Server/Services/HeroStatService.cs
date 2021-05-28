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
        public Task<IEnumerable<TopListEntryDTO>> GetSpammersAsync(ulong steamId, int take = 25);

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
        /// Gets the list of top spammers from a single player's friends list
        /// </summary>        
        public async Task<IEnumerable<TopListEntryDTO>> GetSpammersAsync(ulong steamId, int take = 25)
        {
            // Playtime should not be refreshed, as it will not be required and it would take a lot of extra time...
            // This way this can be done in a single HTTP GET towards Steam API
            var playerDTOs = await _steamService.GetFriendsAsync(steamId, includePlayer: true, includePlaytime: false);
            var tasks = new List<Task<IEnumerable<HeroStatDTO>>>();

            var players = new List<IDotaPlayer>();
            var playersToUpdate = new List<IDotaPlayer>();

            players.AddRange(await _unitOfWork.Users.GetUsersBySteamIdAsync(playerDTOs.Select(p => p.SteamId)));
            players.AddRange(await _unitOfWork.ExternalUsers.GetExternalUsersBySteamIdAsync(playerDTOs.Select(p => p.SteamId)));
            // ids that were not found in either should not exists: they should have all been created at the landing page

            var upToDateStats = new List<TopListEntryDTO>();
            foreach(var player in players)
            {
                // get their hero stats                    
                if (!player.HeroStatsTimestamp.HasValue || player.HeroStatsTimestamp < DateTime.Now.AddHours(-24))
                {
                    // if they are up-to-date, they will be in the database
                    var stats = await _unitOfWork.HeroStats.GetHeroStatsBySteamIdAsync(player.SteamId);
                    upToDateStats.AddRange(stats.Select(s => new TopListEntryDTO
                    {
                        Friend = player.ToDTO(),
                        Hero = s.Hero,
                        Value = s.Games,
                        MatchId = null
                    }));
                }
                else
                {
                    // else they shall be updated
                    playersToUpdate.Add(player);
                }
            }

            var updatedStatsGroupedByPlayer = (await _openDotaService.GetHeroStatsAsync(playersToUpdate)).GroupBy(stat => stat.User.SteamId);

            foreach(var group in updatedStatsGroupedByPlayer)
            {
                IDotaPlayer player = group.First().User;
                var availableHeroStats = await _unitOfWork.HeroStats.GetHeroStatsBySteamIdAsync(player.SteamId);
                var createdHeroStats = new List<HeroStat>();
                foreach(var stat in group)
                {
                    if(availableHeroStats.Any(s => s.Hero == stat.Hero))
                    {
                        // we already had an entity for this hero, update that entity with the fresh value
                        availableHeroStats.Single(s => s.Hero == stat.Hero).Games = stat.Games;
                    }
                    else
                    {
                        // we had no entity for this hero, so create a new one
                        createdHeroStats.Add(new HeroStat(stat, player.SteamId));
                    }
                }

                await _unitOfWork.HeroStats.AddRangeAsync(createdHeroStats);
                player.HeroStatsTimestamp = DateTime.Now;

                // Then add these statistics to the list of statistics to be returned
                upToDateStats.AddRange(group.Select(stat => new TopListEntryDTO
                {
                    Friend = stat.User.ToDTO(),
                    Hero = stat.Hero,
                    Value = stat.Games,
                    MatchId = null
                }));
            }
                       
            _unitOfWork.SaveChanges();

            var ordered = upToDateStats.OrderByDescending(s => s.Value);
            var topValues = ordered.Take(take);
            if(!topValues.Any(v => v.Friend.IsCurrentPlayer) && ordered.Any(v => v.Friend.IsCurrentPlayer))
            {                
                topValues = topValues.Append(ordered.First(v => v.Friend.IsCurrentPlayer));
            }
            return topValues.OrderByDescending(s => s.Value);
        }

        /// <summary>
        /// Gets the list of top players for a single Hero, from a single player's friends list
        /// </summary>        
        public async Task<IEnumerable<TopListEntryDTO>> GetTopByHeroAsync(ulong steamId, Hero hero, int take = 10)
        {
            // Playtime should not be refreshed, as it will not be required and it would take a lot of extra time...
            // This way this can be done in a single HTTP GET towards Steam API
            var playerDTOs = await _steamService.GetFriendsAsync(steamId, includePlayer: true, includePlaytime: false);
            var heroStats = new List<TopListEntryDTO>();
            var tasks = new List<Task<IEnumerable<HeroStatDTO>>>();

            foreach (var playerDTO in playerDTOs)
            {
                IDotaPlayer player = await _unitOfWork.Users.GetUserBySteamIdAsync(steamId);
                if (player == null)
                {
                    player = await _unitOfWork.ExternalUsers.GetAsync(steamId);
                }

                if (player != null)
                {
                    // get their hero stats                    
                    if (!player.HeroStatsTimestamp.HasValue || player.HeroStatsTimestamp < DateTime.Now.AddHours(-24))
                    {
                        // if they are up-to-date, they will be in the database
                        var upToDateStats = await _unitOfWork.HeroStats.GetHeroStatsBySteamIdAsync(player.SteamId);
                        heroStats.AddRange(upToDateStats.Select(s => new TopListEntryDTO
                        {
                            Friend = player.ToDTO(),
                            Hero = s.Hero,
                            Value = s.Games,
                            MatchId = null
                        }));
                    }
                    else
                    {
                        // they are not up-to-date, so we need this player's hero stats from opendota!
                        tasks.Add(_openDotaService.GetHeroStatsAsync(player));
                    }

                }
            }

            // Get the hero stats for the players who were not up-to-date 
            var numberOfBatches = (int)Math.Ceiling((double)tasks.Count() / _openDotaOptions.BatchSize); // run the parallel requests in batches            

            for (int i = 0; i < numberOfBatches; i++)
            {
                var batch = tasks.Skip(i * _openDotaOptions.BatchSize).Take(_openDotaOptions.BatchSize);
                var results = (await Task.WhenAll(batch));
                foreach (var playerResult in results)
                {
                    if (playerResult.Any())
                    {
                        // We have fresh hero statistics from OpenDota
                        // First save these up-to-date values to the local database
                        IDotaPlayer player = playerResult.First().User;
                        var availableStats = await _unitOfWork.HeroStats.GetHeroStatsBySteamIdAsync(player.SteamId);
                        var createdStats = new List<HeroStat>();

                        foreach (var stat in playerResult)
                        {
                            if (availableStats.Any(s => s.Hero == stat.Hero))
                            {
                                // we already had an entity for this hero, update that entity with the fresh value
                                availableStats.Single(s => s.Hero == stat.Hero).Games = stat.Games;
                            }
                            else
                            {
                                // we had no entity for this hero, so create a new one
                                createdStats.Add(new HeroStat(stat, player.SteamId));
                            }
                        }

                        await _unitOfWork.HeroStats.AddRangeAsync(createdStats);
                        player.HeroStatsTimestamp = DateTime.Now;

                        // Then add these statistics to the list of statistics to be returned
                        heroStats.AddRange(playerResult.Select(stat => new TopListEntryDTO
                        {
                            Friend = stat.User.ToDTO(),
                            Hero = stat.Hero,
                            Value = stat.Games,
                            MatchId = null
                        }));
                    }
                }
            }
            _unitOfWork.SaveChanges();

            var filtered = heroStats.Where(stat => stat.Hero == hero && stat.Value > 0);
            var ordered = heroStats.OrderByDescending(s => s.Value);            
            var topValues = ordered.Take(take);
            if (!topValues.Any(v => v.Friend.IsCurrentPlayer))
            {
                var currentPlayerStat = heroStats.SingleOrDefault(stat => stat.Friend.IsCurrentPlayer && stat.Hero == hero);
                if (currentPlayerStat == null)
                {
                    currentPlayerStat = new TopListEntryDTO
                    {
                        Friend = playerDTOs.Single(p => p.IsCurrentPlayer = true), // must exist, because earlier we set includePlayer: true
                        Hero = hero,
                        Value = 0
                    };
                }
                topValues = topValues.Append(currentPlayerStat);
            }
            return topValues.OrderByDescending(s => s.Value);
        }
    }
}
