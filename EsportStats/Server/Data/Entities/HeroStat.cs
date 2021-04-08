using EsportStats.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data.Entities
{
    public class HeroStat
    {
        [Key]
        public int Id { get; set; }

        // This could be either an ApplicationUser.SteamId or an ExternalUser.SteamId
        [Required]
        public ulong SteamId { get; set; }

        [Required]
        public Hero Hero{ get; set; }

        /// <summary>
        /// Amount of games played on the hero.
        /// </summary>
        [Required]
        public int Games { get; set; }        
    }
}
