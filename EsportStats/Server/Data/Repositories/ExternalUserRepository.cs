using EsportStats.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data.Repositories
{
    public class ExternalUserRepository : Repository<ExternalUser, ulong>, IExternalUserRepository
    {
        public ApplicationDbContext AppDbContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public ExternalUserRepository(ApplicationDbContext context) : base(context)
        {
        }                
        
        public async Task<IEnumerable<ExternalUser>> GetExternalUsersBySteamIdAsync(IEnumerable<ulong> steamIds, bool includeTopListEntries = false)
        {
            if (includeTopListEntries)
            {
                return await AppDbContext.ExternalUsers.Include(u => u.TopListEntries).Where(u => steamIds.Contains(u.SteamId)).ToListAsync();
            }
            else
            {
                return await AppDbContext.ExternalUsers.Where(u => steamIds.Contains(u.SteamId)).ToListAsync();
            }            
        }

        public async Task<ExternalUser> GetWithTopListEntriesAsync(ulong steamId)
        {
            return await AppDbContext.ExternalUsers.Include(u => u.TopListEntries).SingleOrDefaultAsync(u => u.SteamId == steamId);
        }
    }
}
