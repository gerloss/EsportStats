using EsportStats.Shared.Enums;
using EsportStats.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EsportStats.Server.Data;
using EsportStats.Server.Data.Entities;

namespace EsportStats.Server.Services
{
    public interface ITopListService
    {         
        public Task<IEnumerable<TopListEntryDTO>> GetByMetricForUser(ulong steamId, Metric metric, int take = 25);
    }

    public class TopListService : ITopListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOpenDotaService _openDotaService;
        private readonly ISteamService _steamService;

        public TopListService(
            IUnitOfWork unitOfWork,
            IOpenDotaService openDotaService,
            ISteamService steamService)
        {
            _unitOfWork = unitOfWork;
            _openDotaService = openDotaService;
            _steamService = steamService;
        }

        /// <summary>
        /// Get the top list entries for a single user by a given metric
        /// </summary>        
        public async Task<IEnumerable<TopListEntryDTO>> GetByMetricForUser(ulong steamId, Metric metric, int take = 25)
        {            
            var playerDTOs = await _steamService.GetFriendsAsync(steamId, includePlayer: true, includePlaytime: false);

            var players = new List<IDotaPlayer>();
            players.AddRange(await _unitOfWork.Users.GetUsersBySteamIdAsync(playerDTOs.Select(p => p.SteamId), includeTopListEntries: true));
            players.AddRange(await _unitOfWork.ExternalUsers.GetExternalUsersBySteamIdAsync(playerDTOs.Select(p => p.SteamId), includeTopListEntries: true));
            // ids that were not found in either should not exists: they should have all been created at the landing page

            var playersToUpdate = new List<IDotaPlayer>();

            var upToDateStats = new List<TopListEntryDTO>();
            foreach(var player in players)
            {
                var requiredEntries = player.TopListEntries.Where(e => e.Metric == metric);
                var timestamp = requiredEntries.FirstOrDefault()?.Timestamp; // TODO: is this always null ?????

                if (!requiredEntries.Any() || !timestamp.HasValue || timestamp.Value < DateTime.Now.AddDays(-24))
                {
                    playersToUpdate.Add(player);
                    
                    _unitOfWork.TopListEntries.RemoveRange(requiredEntries); // remove the old entries, new ones in the response will be saved later
                }
                else 
                {
                    upToDateStats.AddRange(requiredEntries.Select(e =>  new TopListEntryDTO { 
                        Friend = player.ToDTO(player.SteamId == steamId),
                        Hero = e.Hero.Value,
                        Value = e.Value,
                        MatchId = e.MatchId
                    }));                    
                }
            }

            var updatedEntriesGroupedByPlayer = (await _openDotaService.GetTopListEntriesAsync(playersToUpdate, metric)).GroupBy(e => e.User);

            foreach(var group in updatedEntriesGroupedByPlayer)
            {
                IDotaPlayer player = group.First().User; // TODO: is the user in here?????
                var createdEntries = group.Select(e => String.IsNullOrEmpty(player.Guid) ? new TopListEntry(e, metric, player.SteamId) : new TopListEntry(e, metric, player.Guid));
                await _unitOfWork.TopListEntries.AddRangeAsync(createdEntries);
                upToDateStats.AddRange(createdEntries.Select(e => new TopListEntryDTO
                {
                    Friend = player.ToDTO(player.SteamId == steamId),
                    Hero = e.Hero.Value,
                    Value = e.Value,
                    MatchId = e.MatchId
                }));
            }

            _unitOfWork.SaveChanges();

            var ordered = upToDateStats.OrderByDescending(e => e.Value);
            var topValues = ordered.Take(take);

            if (!topValues.Any(v => v.Friend.IsCurrentPlayer) && ordered.Any(v => v.Friend.IsCurrentPlayer))
            {
                topValues = topValues.Append(ordered.First(v => v.Friend.IsCurrentPlayer));
            }

            return topValues.OrderByDescending(e => e.Value);
        }
    }
}
