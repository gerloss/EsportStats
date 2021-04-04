using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Common
{
    public class SteamGameStatsExtDTO
    {
        [JsonProperty("response")]
        public SteamGameStatsResponseDTO Response { get; set; }
    }

    public class SteamGameStatsResponseDTO
    {
        [JsonProperty("game_count")]
        public int GameCount { get; set; }

        [JsonProperty("games")]
        public IEnumerable<SteamGameStatDTO> Games { get; set; }
    }

    public class SteamGameStatDTO
    {
        [JsonProperty("appid")]
        public int AppId { get; set; }

        /// <summary>
        /// Amount of time played in minutes.
        /// </summary>
        [JsonProperty("playtime_forever")]
        public int PlaytimeForeverMinutes { get; set; }
    }
}
