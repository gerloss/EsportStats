using EsportStats.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data.Repositories
{
    public class ExternalUserRepository : Repository<ExternalUser>, IExternalUserRepository
    {
        public ApplicationDbContext AppDbContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public ExternalUserRepository(ApplicationDbContext context) : base(context)
        {
        }                

        public async Task<ExternalUser> GetExternalUserBySteamIdAsync(ulong steamId)
        {
            return await AppDbContext.ExternalUsers.SingleOrDefaultAsync(u => u.SteamId == steamId);
        }

        public async Task<IEnumerable<ExternalUser>> GetExternalUsersBySteamIdAsync(IEnumerable<ulong> steamIds)
        {
            return await AppDbContext.ExternalUsers.Where(u => steamIds.Contains(u.SteamId)).ToListAsync();
        }
    }
}
