using EsportStats.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Services
{
    public interface ISteamFriendService
    {
        public Task<IEnumerable<SteamFriendDTO>> GetFriendsAsync();
        public Task<IEnumerable<SteamFriendDTO>> GetFriendsAsync(string userId);
    }

    public class SteamFriendService : ISteamFriendService
    {
        /// <summary>
        /// Serves the friends of the currently authenticated user.
        /// </summary>        
        public async Task<IEnumerable<SteamFriendDTO>> GetFriendsAsync()
        {
            // Mocked data. TODO: use Db/External api calls
            IEnumerable<SteamFriendDTO> friends = Enumerable.Range(1, 10).Select(x => new SteamFriendDTO
            {
                Username = $"Friend #{x}",
                ImageUrl = "http://placehold.it/160x160",
                HoursPlayed = x * 242,
                LastOnline = DateTime.Now.AddDays(-1 * x).AddHours(-2 * x)
            });

            // It would make sense to check how up-to-date the data stored in the local db is...
            // If its fresh enough we can serve from our own db. (Call to the 'SteamFriendManager' in the DAL.)
            // If its outdated, we make a call to the external API (Steam or OpenDota)...

            return friends;
        }

        /// <summary>
        /// Serves the friends of the user with the id {userId}.
        /// </summary>        
        public async Task<IEnumerable<SteamFriendDTO>> GetFriendsAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
