using EsportStats.Server.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data.Repositories
{
    public interface IExternalUserRepository : IRepository<ExternalUser>
    {
        Task<ExternalUser> GetExternalUserBySteamIdAsync(ulong steamId);
        Task<IEnumerable<ExternalUser>> GetExternalUsersBySteamIdAsync(IEnumerable<ulong> steamIds);

    }
}
