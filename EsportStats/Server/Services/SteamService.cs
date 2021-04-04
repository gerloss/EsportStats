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
        public Task<IEnumerable<SteamUserDTO>> GetFriendsAsync(ulong steamId);
        public Task<SteamProfileExtDTO> GetSteamProfileExternalAsync(ulong steamId);
        public Task<IEnumerable<SteamProfileExtDTO>> GetSteamProfilesExternalAsync(IEnumerable<ulong> steamIds);
        public Task<IEnumerable<ulong>> GetSteamFriendsExternalAsync(ulong steamId);
        public Task<int> GetSteamPlaytimeMinutesAsync(ulong steamId);
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
        /// Serves the friends of the user with the id {userId}.
        /// </summary>        
        public async Task<IEnumerable<SteamUserDTO>> GetFriendsAsync(ulong steamId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the lost of the user's steam friends' ids.
        /// </summary>        
        public async Task<IEnumerable<ulong>> GetSteamFriendsExternalAsync(ulong steamId)
        {
            var steamOptions = new SteamOptions();
            _cfg.GetSection(SteamOptions.Steam).Bind(steamOptions);

            var friendsListUrl = $"https://api.steampowered.com/ISteamUser/GetFriendList/v0001/?key={steamOptions.Key}&steamid={steamId}&relationship=friend";

            var httpClient = _httpClientFactory.CreateClient();
            var friendsListResponse = await httpClient.GetAsync(friendsListUrl);
            var response = await friendsListResponse.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<SteamFriendsListExtDTO>(response);

            return parsedResponse.FriendsList.Friends.Select(f => f.SteamId).ToList();
        }

        /// <summary>
        /// Gets the amount of playtime spent on Dota2 for the user.
        /// </summary>        
        public async Task<int> GetSteamPlaytimeMinutesAsync(ulong steamId)
        {
            var steamOptions = new SteamOptions();
            _cfg.GetSection(SteamOptions.Steam).Bind(steamOptions);

            var playtimeUrl = "https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/"
                            + "?key=" + steamOptions.Key
                            + "&steamid=" + steamId
                            + "&appids_filter=" + steamOptions.AppId
                            + "&include_played_free_games=true&include_appinfo=false";

            var httpClient = _httpClientFactory.CreateClient();
            var playtimeResponse = await httpClient.GetAsync(playtimeUrl);
            var response = await playtimeResponse.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<SteamGameStatsExtDTO>(response);

            return parsedResponse.Response.Games.SingleOrDefault(g => g.AppId == steamOptions.AppId)?.PlaytimeForeverMinutes ?? 0;
        }

        /// <summary>
        /// Gets the user profile from the Steam API.
        /// </summary>
        public async Task<SteamProfileExtDTO> GetSteamProfileExternalAsync(ulong steamId)
        {

            var steamOptions = new SteamOptions();
            _cfg.GetSection(SteamOptions.Steam).Bind(steamOptions);
            var key = steamOptions.Key;
            var playerInfoUrl = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={key}&steamids={steamId}";

            var httpClient = _httpClientFactory.CreateClient();
            var steamInfoResponse = await httpClient.GetAsync(playerInfoUrl);

            var response = await steamInfoResponse.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<SteamExtDTO>(response);

            return parsedResponse.Response.Players.FirstOrDefault();
        }

        /// <summary>
        /// Gets the users profiles from the Steam API.
        /// </summary>
        public async Task<IEnumerable<SteamProfileExtDTO>> GetSteamProfilesExternalAsync(IEnumerable<ulong> steamIds)
        {

            var steamOptions = new SteamOptions();
            _cfg.GetSection(SteamOptions.Steam).Bind(steamOptions);
            var key = steamOptions.Key;
            var joinedIds = String.Join(',', steamIds);
            var playerInfoUrl = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={key}&steamids={joinedIds}";

            var httpClient = _httpClientFactory.CreateClient();
            var steamInfoResponse = await httpClient.GetAsync(playerInfoUrl);

            var response = await steamInfoResponse.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<SteamExtDTO>(response);

            return parsedResponse.Response.Players;
        }
    }
}
