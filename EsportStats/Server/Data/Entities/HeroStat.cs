using EsportStats.Server.Common;
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
        public HeroStat()
        {

        }

        public HeroStat(HeroStatDTO dto, ulong steamId)
        {
            this.SteamId = steamId;
            this.Games = dto.Games;
            // In the DTO it comes as an int stored in a string from the external api
            // First parse it as an int and then convert into Hero Enum
            this.Hero = dto.Hero;
        }

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
