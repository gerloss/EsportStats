using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EsportStats.Shared.DTO;
using EsportStats.Shared.Enums;
using Microsoft.AspNetCore.Components;

namespace EsportStats.Client.Pages
{

    public class MetricSelection
    {
        public Metric SelectedMetric { get; set; }
    }

    public partial class Lists : ComponentBase
    {
        MetricSelection selection = new MetricSelection();

        // Mocked data    
        IEnumerable<TopListEntry> entries = Enumerable.Range(1, 10).Select(x => new TopListEntry
        {
            Friend = new SteamFriend
            {        
                Username = $"Friend #{x}",
                ImageUrl = "http://placehold.it/160x160",
                HoursPlayed = 0,
                LastOnline = null
            },
            HeroThumbnail = "http://placehold.it/59x33",
            MatchId = x,
            Value = x * 1234
        });

    }
    
}
