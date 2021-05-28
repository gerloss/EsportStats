using EsportStats.Server.Data.Entities;
using EsportStats.Shared.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Common
{
    public class HeroStatDTO
    {
        [JsonIgnore]
        public IDotaPlayer User { get; set; }

        [JsonProperty("hero_id")]
        public Hero Hero { get; set; }
        [JsonProperty("last_played")]
        public int LastPlayed { get; set; } 
        [JsonProperty("games")]
        public int Games { get; set; }
        [JsonProperty("win")]
        public int Wins { get; set; }
        [JsonProperty("with_games")]
        public int GamesWith { get; set; }
        [JsonProperty("with_win")]
        public int WinsWith { get; set; }
        [JsonProperty("against_games")]
        public int GamesAgainst { get; set; }
        [JsonProperty("against_win")]
        public int WinsAgainst { get; set; }
    }
}
