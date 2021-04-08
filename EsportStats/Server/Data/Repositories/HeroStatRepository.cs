using EsportStats.Server.Data.Entities;
using EsportStats.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data.Repositories
{
    public class HeroStatRepository : Repository<HeroStat, int>, IHeroStatRepository
    {
        public ApplicationDbContext AppDbContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public HeroStatRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<HeroStat>> GetHeroStatsBySteamIdAsync(ulong steamId)
        {
            return await AppDbContext.HeroStats.Where(s => s.SteamId == steamId).ToListAsync();
        }

        public async Task<IEnumerable<HeroStat>> GetStatsForHeroAsync(Hero hero, IEnumerable<ulong> steamIds)
        {
            return await AppDbContext.HeroStats.Where(stat => stat.Hero == hero && steamIds.Contains(stat.SteamId)).ToListAsync();
        }
    }
}
