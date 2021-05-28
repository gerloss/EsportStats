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
                    // if they are not up-to-date, they should be updated
                    playersToUpdate.Add(player);                    
                }
                else
                {
                    // if they are up-to-date, they will be in the database
                    var stats = await _unitOfWork.HeroStats.GetHeroStatsBySteamIdAsync(player.SteamId);
                    upToDateStats.AddRange(stats.Select(s => new TopListEntryDTO
                    {
                        Friend = player.ToDTO(player.SteamId == steamId),
                        Hero = s.Hero,
                        Value = s.Games,
                        MatchId = null
                    }));
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
                playersToUpdate.SingleOrDefault(p => p.SteamId == player.SteamId).HeroStatsTimestamp = DateTime.Now;                

                // Then add these statistics to the list of statistics to be returned
                upToDateStats.AddRange(group.Select(stat => new TopListEntryDTO
                {
                    Friend = stat.User.ToDTO(stat.User.SteamId == steamId),
                    Hero = stat.Hero,
                    Value = stat.Games,
                    MatchId = null
                }));
            }
                       
            _unitOfWork.SaveChanges();

            var ordered = upToDateStats.OrderByDescending(s => s.Value);
            var topValues = ordered.Take(take);

            if (!topValues.Any(v => v.Friend.IsCurrentPlayer) && ordered.Any(v => v.Friend.IsCurrentPlayer))
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
            var tasks = new List<Task<IEnumerable<HeroStatDTO>>>();

            var players = new List<IDotaPlayer>();
            var playersToUpdate = new List<IDotaPlayer>();

            players.AddRange(await _unitOfWork.Users.GetUsersBySteamIdAsync(playerDTOs.Select(p => p.SteamId)));
            players.AddRange(await _unitOfWork.ExternalUsers.GetExternalUsersBySteamIdAsync(playerDTOs.Select(p => p.SteamId)));
            // ids that were not found in either should not exists: they should have all been created at the landing page

            var upToDateStats = new List<TopListEntryDTO>();
            foreach (var player in players)
            {
                // get their hero stats                    
                if (!player.HeroStatsTimestamp.HasValue || player.HeroStatsTimestamp < DateTime.Now.AddHours(-24))
                {
                    // if they are not up-to-date, they should be updated
                    playersToUpdate.Add(player);
                }
                else
                {
                    // if they are up-to-date, they will be in the database
                    var stats = await _unitOfWork.HeroStats.GetHeroStatsBySteamIdAsync(player.SteamId);
                    upToDateStats.AddRange(stats.Select(s => new TopListEntryDTO
                    {
                        Friend = player.ToDTO(player.SteamId == steamId),
                        Hero = s.Hero,
                        Value = s.Games,
                        MatchId = null
                    }));
                }
            }

            var updatedStatsGroupedByPlayer = (await _openDotaService.GetHeroStatsAsync(playersToUpdate)).GroupBy(stat => stat.User.SteamId);

            foreach (var group in updatedStatsGroupedByPlayer)
            {
                IDotaPlayer player = group.First().User;
                var availableHeroStats = await _unitOfWork.HeroStats.GetHeroStatsBySteamIdAsync(player.SteamId);
                var createdHeroStats = new List<HeroStat>();
                foreach (var stat in group)
                {
                    if (availableHeroStats.Any(s => s.Hero == stat.Hero))
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
                    Friend = stat.User.ToDTO(stat.User.SteamId == steamId),
                    Hero = stat.Hero,
                    Value = stat.Games,
                    MatchId = null
                }));
            }

            _unitOfWork.SaveChanges();

            var filtered = upToDateStats.Where(stat => stat.Hero == hero && stat.Value > 0);
            var ordered = filtered.OrderByDescending(s => s.Value);            
            var topValues = ordered.Take(take);
            if (!topValues.Any(v => v.Friend.IsCurrentPlayer))
            {
                var currentPlayerStat = filtered.SingleOrDefault(stat => stat.Friend.IsCurrentPlayer);
                if (currentPlayerStat == null)
                {
                    currentPlayerStat = new TopListEntryDTO
                    {
                        Friend = playerDTOs.First(p => p.IsCurrentPlayer = true), // must exist, because earlier we set includePlayer: true
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
