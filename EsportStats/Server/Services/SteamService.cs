using EsportStats.Server.Common;
using EsportStats.Shared.DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EsportStats.Server.Services
{
    public interface ISteamService
    {
        public Task<IEnumerable<SteamUserDTO>> GetFriendsAsync();
        public Task<IEnumerable<SteamUserDTO>> GetFriendsAsync(ulong steamId);
        public Task<SteamProfileExtDTO> GetSteamProfileExternalAsync(ulong steamId);        

    }

    public class SteamService : ISteamService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _cfg;

        public SteamService(
            IHttpClientFactory httpClientFactory,
            IConfiguration cfg)
        {
            _httpClientFactory = httpClientFactory;
            _cfg = cfg;
        }

        /// <summary>
        /// Serves the friends of the currently authenticated user.
        /// </summary>        
        public async Task<IEnumerable<SteamUserDTO>> GetFriendsAsync()
        {
            // Mocked data. TODO: use Db/External api calls
            IEnumerable<SteamUserDTO> friends = Enumerable.Range(1, 10).Select(x => new SteamUserDTO
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
        public async Task<IEnumerable<SteamUserDTO>> GetFriendsAsync(ulong steamId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the user profile from the Steam API.
        /// </summary>
        public async Task<SteamProfileExtDTO> GetSteamProfileExternalAsync(ulong steamId)
        {            

            var steamOptions = new SteamOptions();
            _cfg.GetSection(SteamOptions.Steam).Bind(steamOptions);
            var key = steamOptions.Key;
            var playerInfoUrl = $"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={key}&steamids={steamId}";

            var httpClient = _httpClientFactory.CreateClient();
            var steamInfoResponse = await httpClient.GetAsync(playerInfoUrl);

            var response = await steamInfoResponse.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<SteamExtDTO>(response);

            return parsedResponse.Response.Players.FirstOrDefault();
        }
    }
}
