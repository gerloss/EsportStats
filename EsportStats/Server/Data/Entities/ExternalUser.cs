using EsportStats.Server.Common;
using EsportStats.Shared.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data.Entities
{
    /// <summary>
    /// Contains Steam User data for players that DO NOT have a user created in this app.
    /// </summary>
    public class ExternalUser : IDotaPlayer
    {
        public ExternalUser()
        {
        }

        public ExternalUser(SteamProfileExtDTO dto)
        {
            this.SteamId = dto.SteamId;
            this.Name = dto.Name;
            this.ProfileUrl = dto.ProfileUrl;
            this.Avatar = dto.Avatar;
            this.AvatarFull = dto.AvatarFull;
            this.Playtime = null;
            this.Timestamp = DateTime.Now;
        }


        /// <summary>
        /// 64bit SteamID of the user.
        /// </summary>
        [Key]
        [Required]
        public ulong SteamId { get; set; }

        /// <summary>
        /// Public Steam persona name (nickname).
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The full URL of the player's Steam Community profile.
        /// </summary>
        [Required]
        public string ProfileUrl { get; set; }

        /// <summary>
        /// The full URL of the player's 32x32px avatar.
        /// </summary>
        [Required]
        public string Avatar { get; set; }

        /// <summary>
        /// The full URL of the player's 184x184px avatar.
        /// </summary>
        [Required]
        public string AvatarFull { get; set; }

        /// <summary>
        /// The date of the last update of this user's Steam data.
        /// </summary>
        [Required]
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


        public void UpdateFromExternalProfile(SteamProfileExtDTO dto)
        {
            this.Name = dto.Name;
            this.ProfileUrl = dto.ProfileUrl;
            this.Avatar = dto.Avatar;
            this.AvatarFull = dto.AvatarFull;
            this.Timestamp = DateTime.Now;                      
        }

        public SteamUserDTO ToDTO(bool isCurrentUser = false)
        {
            return new SteamUserDTO
            {
                Name = this.Name,
                ProfileUrl = this.ProfileUrl,
                Avatar = this.Avatar,
                AvatarFull = this.AvatarFull,
                Playtime = this.Playtime,
                SteamId = this.SteamId,
                IsCurrentPlayer = isCurrentUser
            };
        }

        public void SetPlaytime(int minutes)
        {
            this.Playtime = minutes;
            this.PlaytimeTimestamp = DateTime.Now;
        }
    }
}
