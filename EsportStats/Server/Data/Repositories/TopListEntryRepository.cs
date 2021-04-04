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

        public Task<IEnumerable<TopListEntry>> GetTopEntriesByMetricAsync(Metric metric, int count = 20, ulong? steamId = null)
        {
            // TODO: Optional filtering for steamId
            //return await AppDbContext.TopListEntries
            //    .Where(e => e.Metric == metric)
            //    .OrderByDescending(e => e.Value)
            //    .Take(count)
            //    .ToListAsync();
            
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TopListEntry>> GetTopEntriesForSteamIdAsync(ulong steamId)
        {
            return await AppDbContext.TopListEntries.Include(e => e.User).Where(e => e.ExternalUserId == steamId || e.User.SteamId == steamId).ToListAsync();
        }
    }
}
