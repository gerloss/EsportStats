using System;
using System.Collections.Generic;
using System.Text;

namespace EsportStats.Shared.DTO
{
    public class SteamUserDTO
    {
        /// <summary>
        /// Steam username.
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
        /// Minutes spent in Dota 2.
        /// </summary>
        public int? Playtime { get; set; }
        /// <summary>
        /// is this the currently logged in user's profile?
        /// </summary>
        public bool IsCurrentPlayer { get; set; } = false;
    }
}
