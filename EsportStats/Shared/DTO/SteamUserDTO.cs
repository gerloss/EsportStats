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
        public string Username { get; set; }
        /// <summary>
        /// Steam avatar picture URL.
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// Hours spent in Dota 2.
        /// </summary>
        public int HoursPlayed { get; set; }
        /// <summary>
        /// Date and time of users last online activity.
        /// </summary>
        /// <remarks>Null means user is currently online.</remarks>
        public DateTime? LastOnline { get; set; }
    }
}
