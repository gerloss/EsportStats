using EsportStats.Server.Data.Entities;
using EsportStats.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data.Repositories
{
    public interface ITopListEntryRepository : IRepository<TopListEntry>
    {
        Task<IEnumerable<TopListEntry>> GetTopEntriesByMetricAsync(Metric metric, int count = 20, ulong? steamId = null);
        
        Task<IEnumerable<TopListEntry>> GetTopEntriesForSteamIdAsync(ulong steamId);
    }
}
