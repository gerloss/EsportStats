using EsportStats.Server.Data.Entities;
using EsportStats.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data.Repositories
{
    public interface ITopListEntryRepository : IRepository<TopListEntry, int>
    {      
        Task<IEnumerable<TopListEntry>> GetTopEntriesForSteamIdAsync(ulong steamId);
    }
}
