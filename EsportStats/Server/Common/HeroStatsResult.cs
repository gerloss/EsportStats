using EsportStats.Server.Data.Entities;
using EsportStats.Shared.DTO;
using EsportStats.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Common
{
    public class HeroStatsResult
    {
        public ulong SteamId { get; set; }

        public SteamUserDTO Player { get; set; }        

        public Hero Hero { get; set; }

        public int Value { get; set; }
    }
}
