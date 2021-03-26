using EsportStats.Shared.Enums;
using EsportStats.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Services
{
    public interface ITopListService
    {
        public Task<IEnumerable<TopListEntry>> GetByMetricAsync(Metric metric);
    }

    public class TopListService : ITopListService
    {
        /// <summary>
        /// Serves a list of the top values achieved by the selected metric from the currently authenticated user's friends.
        /// </summary>                
        public async Task<IEnumerable<TopListEntry>> GetByMetricAsync(Metric metric)
        {

            // Mocked data. TODO: use Db/External api calls
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
                MatchId = (int)metric,
                Value = x * 1234
            });

            // It would make sense to check how up-to-date the data stored in the local db is...
            // If its fresh enough we can serve from our own db. (Call to the 'SteamFriendManager' in the DAL.)
            // If its outdated, we make a call to the external API (Steam or OpenDota)...

            return entries;
        }
    }
}
