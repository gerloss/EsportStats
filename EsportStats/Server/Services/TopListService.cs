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
        public Task<IEnumerable<TopListEntryDTO>> GetByMetricAsync(string userId, Metric metric, int take = 25);
        public Task<IEnumerable<TopListEntryDTO>> GetByMetricAsync(ulong steamId, Metric metric, int take = 25);
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
        public async Task<List<TopListEntry>> GetByMetricForUser(string userId, Metric metric)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);
            return await GetByMetricForUser(user.SteamId, metric);
        }

        /// <summary>
        /// Get the top list entries for a single user by a given metric
        /// </summary>        
        public async Task<List<TopListEntry>> GetByMetricForUser(ulong steamId, Metric metric)
        {
            var player = await _unitOfWork.Users.GetUserBySteamIdAsync(steamId, includeTopListEntries: true);
            ExternalUser extPlayer = null;
            if (player == null)
            {
                extPlayer = await _unitOfWork.ExternalUsers.GetWithTopListEntriesAsync(steamId);
                if (extPlayer == null)
                {
                    // If there are any valid toplist entries for this user, then the user should already exist, because they have been persisted during loading the front page.
                    // So this probably means that no entries were available externally.
                    return new List<TopListEntry>();
                }
            }

            var stats = player != null ? player.TopListEntries : extPlayer.TopListEntries;
            stats = stats.Where(e => e.Metric == metric).OrderByDescending(e => e.Value).ToList();
            DateTime? timestamp = stats.FirstOrDefault()?.Timestamp;

            if (!timestamp.HasValue || timestamp < DateTime.Now.AddHours(-24))
            {
                // Stats are not up to date, replace them with fresh data from the opendota api
                
                // Remove the outdated entries
                _unitOfWork.TopListEntries.RemoveRange(stats);

                // Create fresh entities for the newly requested entries
                var upToDateStats = await _openDotaService.GetTopListEntriesAsync(steamId, metric);
                var createdStats = new List<TopListEntry>();                
                foreach(var dto in upToDateStats)
                {
                    var entity = player != null ? new TopListEntry(dto, metric, player.Id)  : new TopListEntry(dto, metric, extPlayer.SteamId);
                    createdStats.Add(entity);
                }

                await _unitOfWork.TopListEntries.AddRangeAsync(createdStats);

                _unitOfWork.SaveChanges();

                return createdStats;
            }
            else
            {
                return stats;
            }
        }


        /// <summary>
        /// Serves a list of the top values achieved by the selected metric from the given authenticated user's friends.
        /// </summary>                
        public async Task<IEnumerable<TopListEntryDTO>> GetByMetricAsync(string userId, Metric metric, int take = 25)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);
            return await GetByMetricAsync(user.SteamId, metric, take);
        }

        /// <summary>
        /// Serves a list of the top values achieved by the selected metric from the given authenticated user's friends.
        /// </summary>                
        public async Task<IEnumerable<TopListEntryDTO>> GetByMetricAsync(ulong steamId, Metric metric, int take = 25)
        {
            throw new NotImplementedException();
        }

    }
}
