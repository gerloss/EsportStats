using EsportStats.Shared.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Common
{
    public class TopListEntryExtDTO
    {
        [JsonProperty("hero_id")]
        public Hero Hero { get; set; }

        [JsonProperty("match_id")]
        public ulong MatchId { get; set; }

        // One of these values will be populated depending on the requested metric
        [Display(Name = "APM", ShortName = "actions_per_min")]
        public int? actions_per_min { get; set; }
        [Display(Name = "Assists", ShortName = "assists")]
        public int? assists { get; set; }
        [Display(Name = "Biggest Comeback", ShortName = "comeback")]
        public int? comeback { get; set; }
        [Display(Name = "Courier Kills", ShortName = "courier_kills")]
        public int? courier_kills { get; set; }
        [Display(Name = "Deaths", ShortName = "deaths")]
        public int? deaths { get; set; }
        [Display(Name = "Denies", ShortName = "denies")]
        public int? denies { get; set; }
        [Display(Name = "Gems Purchased", ShortName = "purchase_gem")]
        public int? purchase_gem { get; set; }
        [Display(Name = "GPM", ShortName = "gold_per_min")]
        public int? gold_per_min { get; set; }
        [Display(Name = "Hero Damage", ShortName = "hero_damage")]
        public int? hero_damage { get; set; }
        [Display(Name = "Hero Healing", ShortName = "hero_healing")]
        public int? hero_healing { get; set; }
        [Display(Name = "KDA", ShortName = "kda")]
        public int? kda { get; set; }
        [Display(Name = "Kills", ShortName = "kills")]
        public int? kills { get; set; }
        [Display(Name = "Lane Efficiency Pct", ShortName = "lane_efficiency_pct")]
        public int? lane_efficiency_pct { get; set; }
        [Display(Name = "Last Hits", ShortName = "last_hits")]
        public int? last_hits { get; set; }
        [Display(Name = "Biggest Loss", ShortName = "loss")]
        public int? loss { get; set; }
        [Display(Name = "Match Duration", ShortName = "duration")]
        public int? duration { get; set; }
        [Display(Name = "Neutral Kills", ShortName = "neutral_kills")]
        public int? neutral_kills { get; set; }
        [Display(Name = "Observer Wards Purchased", ShortName = "purchase_ward_observer")]
        public int? purchase_ward_observer { get; set; }
        [Display(Name = "Amount of Pings", ShortName = "pings")]
        public int? pings { get; set; }
        [Display(Name = "Rapiers Purchased", ShortName = "purchase_rapier")]
        public int? purchase_rapier { get; set; }
        [Display(Name = "Sentry Wards Purchased", ShortName = "purchase_ward_sentry")]
        public int? purchase_ward_sentry { get; set; }
        [Display(Name = "Biggest Stomp", ShortName = "stomp")]
        public int? stomp { get; set; }
        [Display(Name = "Stuns", ShortName = "stuns")]
        public int? stuns { get; set; }
        [Display(Name = "Tower Damage", ShortName = "tower_damage")]
        public int? tower_damage { get; set; }
        [Display(Name = "Tower Kills", ShortName = "tower_kills")]
        public int? tower_kills { get; set; }
        [Display(Name = "TP Scrolls Purchased", ShortName = "purchase_tpscroll")]
        public int? purchase_tpscroll { get; set; }
        [Display(Name = "XPM", ShortName = "xp_per_min")]
        public int? xp_per_min { get; set; }       

        /// <summary>
        /// Gets the value for the selected Metric
        /// </summary>        
        public int GetValue(Metric m)
        {
            var propertyName = m.GetShortName();            
            int? value = (int?) this.GetType().GetProperty(propertyName).GetValue(this, null);
            return value.HasValue ? value.Value : 0;
        }
    }
}
