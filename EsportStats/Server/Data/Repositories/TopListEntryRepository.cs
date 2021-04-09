using EsportStats.Server.Data.Entities;
using EsportStats.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data.Repositories
{
    public class TopListEntryRepository : Repository<TopListEntry, int>, ITopListEntryRepository
    {
        public ApplicationDbContext AppDbContext
        {
            get { return _context as ApplicationDbContext; }
        }


        public TopListEntryRepository(ApplicationDbContext context)
            : base(context)
        {            
        }

        public async Task<IEnumerable<TopListEntry>> GetTopEntriesForSteamIdAsync(ulong steamId)
        {
            return await AppDbContext.TopListEntries.Include(e => e.User).Where(e => e.ExternalUserId == steamId || e.User.SteamId == steamId).ToListAsync();
        }
    }
}
