using EsportStats.Server.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data.Repositories
{
    public interface IUserRepository : IRepository<ApplicationUser, string>
    {
        Task<ApplicationUser> GetUserBySteamIdAsync(ulong steamId, bool includeTopListEntries = false);
        Task<IEnumerable<ApplicationUser>> GetUsersBySteamIdAsync(IEnumerable<ulong> steamIds);

    }
}
