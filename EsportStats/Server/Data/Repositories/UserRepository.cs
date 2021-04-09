using EsportStats.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data.Repositories
{
    public class UserRepository : Repository<ApplicationUser, string>, IUserRepository
    {
        public ApplicationDbContext AppDbContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ApplicationUser> GetUserBySteamIdAsync(ulong steamId, bool includeTopListEntries = false)
        {
            if (includeTopListEntries)
            {
                return await AppDbContext.Users.Include(u => u.TopListEntries).SingleOrDefaultAsync(u => u.SteamId == steamId);
            }
            else
            {
                return await AppDbContext.Users.SingleOrDefaultAsync(u => u.SteamId == steamId);
            }
            
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersBySteamIdAsync(IEnumerable<ulong> steamIds)
        {
            return await AppDbContext.Users.Where(u => steamIds.Contains(u.SteamId)).ToListAsync();
        }
    }
}
