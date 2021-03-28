using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Common
{
    // DTO based on: https://developer.valvesoftware.com/wiki/Steam_Web_API#Public_Data

    public class SteamExtDTO
    {
        [JsonProperty("response")]
        public SteamProfileResponseDTO Response { get; set; }
    }

    public class SteamProfileResponseDTO
    {
        [JsonProperty("players")]
        public IEnumerable<SteamProfileExtDTO> Players { get; set; }
    }

    /// <summary>
    /// DTO for steam profiles read from the external Steam Api
    /// </summary>
    public class SteamProfileExtDTO
    {
        /// <summary>
        /// 64bit SteamID of the user.
        /// </summary>
        [JsonProperty("steamid")]
        public ulong SteamId { get; set; }

        /// <summary>
        /// Public Steam persona name (nickname).
        /// </summary>
        [JsonProperty("personaname")]
        public string Name { get; set; }

        /// <summary>
        /// The full URL of the player's Steam Community profile.
        /// </summary>
        [JsonProperty("profileurl")]
        public string ProfileUrl { get; set; }

        /// <summary>
        /// The full URL of the player's 32x32px avatar.
        /// </summary>
        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        /// <summary>
        /// The full URL of the player's 184x184px avatar.
        /// </summary>
        [JsonProperty("avatarfull")]
        public string AvatarFull { get; set; }

        /// <summary>
        /// Last time the user was online as unix timestamp
        /// </summary>
        [JsonProperty("lastlogoff")]
        public int LastOnline { get; set; }

    }
}
