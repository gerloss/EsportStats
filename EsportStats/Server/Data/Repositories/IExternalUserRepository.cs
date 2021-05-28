using EsportStats.Server.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data.Repositories
{
    public interface IExternalUserRepository : IRepository<ExternalUser, ulong>
    {
        Task<ExternalUser> GetWithTopListEntriesAsync(ulong steamId);
        Task<IEnumerable<ExternalUser>> GetExternalUsersBySteamIdAsync(IEnumerable<ulong> steamIds, bool includeTopListEntries = false);
    }
}
