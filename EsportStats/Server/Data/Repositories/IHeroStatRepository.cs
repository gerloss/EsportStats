using EsportStats.Server.Data.Entities;
using EsportStats.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data.Repositories
{
    public interface IHeroStatRepository : IRepository<HeroStat, int>
    {
        Task<IEnumerable<HeroStat>> GetHeroStatsBySteamIdAsync(ulong steamId);

        Task<IEnumerable<HeroStat>> GetStatsForHeroAsync(Hero hero, IEnumerable<ulong> steamIds);
    }
}
