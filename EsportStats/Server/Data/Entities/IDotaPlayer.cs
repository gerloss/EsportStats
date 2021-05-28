using EsportStats.Server.Common;
using EsportStats.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data.Entities
{
    public interface IDotaPlayer
    {
        void UpdateFromExternalProfile(SteamProfileExtDTO dto);
        SteamUserDTO ToDTO(bool isCurrentUser = false);
        void SetPlaytime(int minutes);
        
        
        public string Guid { get; }


        /// <summary>
        /// 64bit SteamID of the user.
        /// </summary>
        public ulong SteamId { get; set; }

        /// <summary>
        /// Public Steam persona name (nickname).
        /// </summary>        
        public string Name { get; set; }

        /// <summary>
        /// The full URL of the player's Steam Community profile.
        /// </summary>
        public string ProfileUrl { get; set; }

        /// <summary>
        /// The full URL of the player's 32x32px avatar.
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// The full URL of the player's 184x184px avatar.
        /// </summary>
        public string AvatarFull { get; set; }

        /// <summary>
        /// The date of the last update of this user's Steam data.
        /// </summary>        
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Amount of Dota 2 played in minutes.
        /// </summary>
        public int? Playtime { get; set; }

        /// <summary>
        /// The date of the last update of this user's playtime.
        /// </summary>
        public DateTime? PlaytimeTimestamp { get; set; }

        /// <summary>
        /// The top list entries of this player.
        /// </summary>
        public List<TopListEntry> TopListEntries { get; set; }

        /// <summary>
        /// The date of the last update of this user's hero stats.
        /// </summary>
        public DateTime? HeroStatsTimestamp { get; set; }
    }
}
