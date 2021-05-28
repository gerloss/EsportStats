using EsportStats.Server.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Common
{
    public class HeroStatRequest
    {
        public IDotaPlayer User { get; set; }

        public IEnumerable<HeroStat> Stats{ get; set; }
    }
}
