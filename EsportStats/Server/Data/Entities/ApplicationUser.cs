﻿using EsportStats.Server.Common;
using EsportStats.Shared.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // Additional public data scraped from the Steam Api 
        // More info at: https://developer.valvesoftware.com/wiki/Steam_Web_API#Public_Data

        
        /// <summary>
        /// 64bit SteamID of the user.
        /// </summary>
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
        /// Amount of Dota 2 hours played.
        /// </summary>
        public double? HoursPlayed { get; set; }

        /// <summary>
        /// The date of the last update of this user's Steam data.
        /// </summary>
        [Required]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The top list entries of this user.
        /// </summary>
        public List<TopListEntry> TopListEntries { get; set; }


        public void UpdateFromExternalProfile(SteamProfileExtDTO dto)
        {
            this.Name = dto.Name;
            this.ProfileUrl = dto.ProfileUrl;
            this.Avatar = dto.Avatar;
            this.AvatarFull = dto.AvatarFull;
            this.Timestamp = DateTime.Now;
        }

        public SteamUserDTO ToDTO()
        {
            return new SteamUserDTO
            {
                Name = this.Name,
                ProfileUrl = this.ProfileUrl,
                Avatar = this.Avatar,
                AvatarFull = this.AvatarFull,
                HoursPlayed = this.HoursPlayed,                
            };
        }
    }
}
