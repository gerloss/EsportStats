using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EsportStats.Shared.DTO;
using EsportStats.Shared.Enums;
using Microsoft.AspNetCore.Components;

namespace EsportStats.Client.Pages
{

    public class HeroSelection
    {
        public Hero SelectedHero { get; set; }
    }

    public partial class HeroStats : ComponentBase
    {
        HeroSelection selection = new HeroSelection();

        // Mocked data    
        IEnumerable<TopListEntryDTO> entries = Enumerable.Range(1, 10).Select(x => new TopListEntryDTO
        {
            Friend = new SteamUserDTO
            {
                Username = $"Friend #{x}",
                ImageUrl = "http://placehold.it/160x160",
                HoursPlayed = 0,
                LastOnline = null
            },
            HeroThumbnail = "",
            MatchId = x,
            Value = x * 17
        });

    }

}
