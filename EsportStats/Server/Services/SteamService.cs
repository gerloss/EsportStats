using EsportStats.Server.Common;
using EsportStats.Server.Data;
using EsportStats.Server.Data.Entities;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _cfg;

        public SteamService(
            IHttpClientFactory httpClientFactory,
            IConfiguration cfg,
            IUnitOfWork unitOfWork)
        {
            _httpClientFactory = httpClientFactory;
            _cfg = cfg;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Serves the friends of the user with the id {steamId}.
        /// </summary>        
        public async Task<IEnumerable<SteamUserDTO>> GetFriendsAsync(ulong steamId)
        {
            // Get the list of friend ids
            var friendIds = await GetSteamFriendsExternalAsync(steamId);                        

            // Check if friends have an account
            var applicationUsers = await _unitOfWork.Users.GetUsersBySteamIdAsync(friendIds);
            var appUsersToUpdate = applicationUsers.Where(u => u.Timestamp < DateTime.Now.AddHours(-24));            
            
            // leave the users that were succesfully found
            friendIds = friendIds.Where(id => !applicationUsers.Any(f => f.SteamId == id));

            // Check if remaining friends have an External User
            var externalFriends = await _unitOfWork.ExternalUsers.GetExternalUsersBySteamIdAsync(friendIds);
            var externalsToUpdate = externalFriends.Where(u => u.Timestamp < DateTime.Now.AddHours(-24));

            // leave the users that were succesfully found
            var missingFriendIds = friendIds.Where(id => !externalFriends.Any(f => f.SteamId == id)).ToList();

            var idsToUpdate = new List<ulong>(missingFriendIds);
            idsToUpdate.AddRange(externalsToUpdate.Select(u => u.SteamId));
            idsToUpdate.AddRange(appUsersToUpdate.Select(u => u.SteamId));

            // Get data for users that need to be updated or created in a single request
            var updated = await GetSteamProfilesExternalAsync(idsToUpdate);

            foreach(var appUser in appUsersToUpdate)
            {
                appUser.UpdateFromExternalProfile(updated.Single(u => u.SteamId == appUser.SteamId));
            }

            foreach (var extUser in externalsToUpdate)
            {
                extUser.UpdateFromExternalProfile(updated.Single(u => u.SteamId == extUser.SteamId));
            }

            // Create entities for the players that were not found in the db            
            var createdExternalUsers = updated
                .Where(u => appUsersToUpdate.Any(appUser => appUser.SteamId == u.SteamId)) // remove players that were already found within the ApplicationUsers
                .Where(u => externalsToUpdate.Any(ext => ext.SteamId == u.SteamId))        // remove players that were already found in the ExternalUsers table
                .Select(friendFromSteamApi => new ExternalUser(friendFromSteamApi));       // convert them to new ExternalUser entities
            await _unitOfWork.ExternalUsers.AddRangeAsync(createdExternalUsers);
            _unitOfWork.SaveChanges();

            var friends = new List<SteamUserDTO>();
            friends.AddRange(applicationUsers.Select(u => u.ToDTO()));
            friends.AddRange(externalFriends.Select(u => u.ToDTO()));
            friends.AddRange(createdExternalUsers.Select(u => u.ToDTO()));
            
            //TODO: Handle the playtime values!

            return friends;
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
