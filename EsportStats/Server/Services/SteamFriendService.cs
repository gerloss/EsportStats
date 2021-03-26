using EsportStats.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Services
{
    public interface ISteamFriendService
    {
        public Task<IEnumerable<SteamFriend>> GetFriendsAsync();
        public Task<IEnumerable<SteamFriend>> GetFriendsAsync(string userId);
    }

    public class SteamFriendService : ISteamFriendService
    {
        /// <summary>
        /// Serves the friends of the currently authenticated user.
        /// </summary>        
        public async Task<IEnumerable<SteamFriend>> GetFriendsAsync()
        {
            // Mocked data. TODO: use Db/External api calls
            IEnumerable<SteamFriend> friends = Enumerable.Range(1, 10).Select(x => new SteamFriend
            {
                Username = $"Friend #{x}",
                ImageUrl = "http://placehold.it/160x160",
                HoursPlayed = x * 242,
                LastOnline = DateTime.Now.AddDays(-1 * x).AddHours(-2 * x)
            });

            return friends;
        }

        /// <summary>
        /// Serves the friends of the user with the id {userId}.
        /// </summary>        
        public async Task<IEnumerable<SteamFriend>> GetFriendsAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
