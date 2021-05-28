using EsportStats.Server.Common;
using EsportStats.Server.Data;
using EsportStats.Server.Data.Entities;
using EsportStats.Shared.DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace EsportStats.Server.Services
{
    public interface ISteamService
    {
        public Task<IEnumerable<SteamUserDTO>> GetFriendsAsync(string userId, bool includePlayer = true, bool includePlaytime = true);
        public Task<IEnumerable<SteamUserDTO>> GetFriendsAsync(ulong steamId, bool includePlayer = true, bool includePlaytime = true);
        public Task<SteamProfileExtDTO> GetSteamProfileExternalAsync(ulong steamId);
        public Task<IEnumerable<SteamProfileExtDTO>> GetSteamProfilesExternalAsync(IEnumerable<ulong> steamIds);
        public Task<IEnumerable<ulong>> GetSteamFriendsExternalAsync(ulong steamId);
        public Task<KeyValuePair<ulong, int>> GetSteamPlaytimeMinutesAsync(ulong steamId);
        public Task<Dictionary<ulong, int>> GetSteamPlaytimesMinutesAsync(IEnumerable<ulong> steamIds);
    }

    public class SteamService : ISteamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _httpClientFactory;        
        private readonly SteamOptions _steamOptions;

        public SteamService(
            IHttpClientFactory httpClientFactory,
            SteamOptions steamOptions,
            IUnitOfWork unitOfWork)
        {
            _httpClientFactory = httpClientFactory;
            _steamOptions = steamOptions;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Serves the friends of the user with the given userid.
        /// </summary>        
        public async Task<IEnumerable<SteamUserDTO>> GetFriendsAsync(string userId, bool includePlayer = true, bool includePlaytime = true)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);
            return await GetFriendsAsync(user.SteamId, includePlayer, includePlaytime);
        }

        /// <summary>
        /// Serves the friends of the user with the id {steamId}.
        /// </summary>        
        public async Task<IEnumerable<SteamUserDTO>> GetFriendsAsync(ulong steamId, bool includePlayer = true, bool includePlaytime = true)
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

            var currentUserProfile = await _unitOfWork.Users.GetUserBySteamIdAsync(steamId);
            if (includePlayer && currentUserProfile.Timestamp < DateTime.Now.AddHours(-24))
            {                                
                idsToUpdate.Add(steamId);                
            }

            // Get data for users that need to be updated or created in a single request
            var updated = idsToUpdate.Any() ? await GetSteamProfilesExternalAsync(idsToUpdate) : new List<SteamProfileExtDTO>();

            foreach(var appUser in appUsersToUpdate)
            {
                appUser.UpdateFromExternalProfile(updated.Single(u => u.SteamId == appUser.SteamId));                
            }

            foreach (var extUser in externalsToUpdate)
            {
                extUser.UpdateFromExternalProfile(updated.Single(u => u.SteamId == extUser.SteamId));                                
            }

            if (includePlayer && currentUserProfile.Timestamp < DateTime.Now.AddHours(-24))
            {
                currentUserProfile.UpdateFromExternalProfile(updated.Single(u => u.SteamId == steamId));
            }

            // Create entities for the players that were not found in the db            
            var createdExternalUsers = updated
                .Where(u => !applicationUsers.Any(appUser => appUser.SteamId == u.SteamId)) // remove players that were already found within the ApplicationUsers
                .Where(u => !externalFriends.Any(ext => ext.SteamId == u.SteamId))          // remove players that were already found in the ExternalUsers table
                .Where(u => u.SteamId != steamId)                                           // remove the currently logged in user
                .Select(friendFromSteamApi => new ExternalUser(friendFromSteamApi));        // convert them to new ExternalUser entities

            if (createdExternalUsers.Any())
            {
                await _unitOfWork.ExternalUsers.AddRangeAsync(createdExternalUsers);
            }

            if (includePlaytime)
            {
                // Playtime values can only be handled in individual requests
                // Get all the ids for the people whose playtime must be refreshed
                var outdatedPlaytimeUserIds = applicationUsers
                    .Where(u => !u.PlaytimeTimestamp.HasValue || u.PlaytimeTimestamp.Value < DateTime.Now.AddHours(-24))
                    .Select(u => u.SteamId)
                    .ToList();

                outdatedPlaytimeUserIds.AddRange(externalFriends
                    .Where(u => !u.PlaytimeTimestamp.HasValue || u.PlaytimeTimestamp.Value < DateTime.Now.AddHours(-24))
                    .Select(u => u.SteamId));

                outdatedPlaytimeUserIds.AddRange(createdExternalUsers
                    .Where(u => !u.PlaytimeTimestamp.HasValue || u.PlaytimeTimestamp.Value < DateTime.Now.AddHours(-24))
                    .Select(u => u.SteamId));                
                

                if (includePlayer && (!currentUserProfile.PlaytimeTimestamp.HasValue || currentUserProfile.PlaytimeTimestamp.Value < DateTime.Now.AddHours(-24)))
                {
                    outdatedPlaytimeUserIds.Add(steamId);
                }

                var playTimes = await GetSteamPlaytimesMinutesAsync(outdatedPlaytimeUserIds);                

                foreach (var time in playTimes)
                {
                    if (applicationUsers.Any(u => u.SteamId == time.Key))
                    {
                        applicationUsers.Single(u => u.SteamId == time.Key).SetPlaytime(time.Value);
                    }
                    else if (externalFriends.Any(u => u.SteamId == time.Key))
                    {
                        externalFriends.Single(u => u.SteamId == time.Key).SetPlaytime(time.Value);
                    }
                    else if (createdExternalUsers.Any(u => u.SteamId == time.Key))
                    {
                        createdExternalUsers.Single(u => u.SteamId == time.Key).SetPlaytime(time.Value);
                    }
                    else if (includePlayer && (!currentUserProfile.PlaytimeTimestamp.HasValue || currentUserProfile.PlaytimeTimestamp.Value < DateTime.Now.AddHours(-24)))
                    {
                        currentUserProfile.SetPlaytime(time.Value);
                    }
                }
            }            

            var friends = new List<SteamUserDTO>();
            friends.AddRange(applicationUsers.Select(u => u.ToDTO()));
            friends.AddRange(externalFriends.Select(u => u.ToDTO()));
            friends.AddRange(createdExternalUsers.Select(u => u.ToDTO()));
            if (includePlayer)
            {                
                friends.Add(currentUserProfile.ToDTO(isCurrentUser: true));
            }
                        
            _unitOfWork.SaveChanges();
            return friends.Where(f => f.Playtime.HasValue && f.Playtime.Value > 0);
        }

        /// <summary>
        /// Gets the list of the user's steam friends' ids.
        /// </summary>        
        public async Task<IEnumerable<ulong>> GetSteamFriendsExternalAsync(ulong steamId)
        {
            
            var friendsListUrl = $"https://api.steampowered.com/ISteamUser/GetFriendList/v0001/?key={_steamOptions.Key}&steamid={steamId}&relationship=friend";

            var httpClient = _httpClientFactory.CreateClient();
            var friendsListResponse = await httpClient.GetAsync(friendsListUrl);

            if (!friendsListResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Unsuccessful request towards the Steam API: https://api.steampowered.com/ISteamUser/GetFriendList/v0001/");
            }

            var response = await friendsListResponse.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<SteamFriendsListExtDTO>(response);

            return parsedResponse.FriendsList.Friends.Select(f => f.SteamId).ToList();
        }

        /// <summary>
        /// Gets the amount of playtime spent on Dota2 for the user.
        /// </summary>        
        public async Task<KeyValuePair<ulong, int>> GetSteamPlaytimeMinutesAsync(ulong steamId)
        {            
            var playtimeUrl = "https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/"
                            + "?key=" + _steamOptions.Key
                            + "&steamid=" + steamId
                            + "&appids_filter=" + _steamOptions.AppId
                            + "&include_played_free_games=true&include_appinfo=false";

            var httpClient = _httpClientFactory.CreateClient();
            var playtimeResponse = await httpClient.GetAsync(playtimeUrl);

            if (!playtimeResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Unsuccessful request towards the Steam API: https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/");
            }

            var response = await playtimeResponse.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<SteamGameStatsExtDTO>(response);
            
            if (parsedResponse.Response.GameCount == 0)
            {
                return new KeyValuePair<ulong, int>(steamId, 0);
            }
            else
            {
                var gameStat = parsedResponse.Response.Games.SingleOrDefault(g => g.AppId == _steamOptions.AppId);
                if (gameStat != null)
                {
                    return new KeyValuePair<ulong, int>(steamId, gameStat.PlaytimeForeverMinutes);
                }
                else
                {
                    return new KeyValuePair<ulong, int>(steamId, 0);
                }
            }
        }

        public async Task<Dictionary<ulong, int>> GetSteamPlaytimesMinutesAsync(IEnumerable<ulong> steamIds)
        {
            var playtimeBaseUrl = "https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/";
            var steamIdQueryKey = "steamid";

            var urls = steamIds.Select(id => playtimeBaseUrl
                + "?key=" + _steamOptions.Key
                + "&" + steamIdQueryKey + "=" + id
                + "&appids_filter=" + _steamOptions.AppId
                + "&include_played_free_games=true&include_appinfo=false"
            );

            var httpClient = _httpClientFactory.CreateClient();
            SetMaxConcurrency(playtimeBaseUrl, _steamOptions.BatchSize);

            var numberOfBatches = (int)Math.Ceiling((double)urls.Count() / _steamOptions.BatchSize); // run the parallel requests in batches            

            var results = new Dictionary<ulong, int>();

            for (int i = 0; i < numberOfBatches; i++)
            {
                var batchOfUrls = urls.Skip(i * _steamOptions.BatchSize).Take(_steamOptions.BatchSize);
                var batchOfRequests = batchOfUrls.Select(url => httpClient.GetAsync(url));
                var httpResponses = await Task.WhenAll(batchOfRequests);

                foreach(var response in httpResponses)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException("Unsuccessful request towards the Steam API: https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/");
                    }

                    var responseString = await response.Content.ReadAsStringAsync();
                    var parsedResponse = JsonConvert.DeserializeObject<SteamGameStatsExtDTO>(responseString);
                    ulong steamId = Convert.ToUInt64(HttpUtility.ParseQueryString(response.RequestMessage.RequestUri.Query).Get(steamIdQueryKey)); // there must always be a steamId query parameter, its put in the request a couple of lines above...

                    if (parsedResponse.Response.GameCount == 0)
                    {
                        results.Add(steamId, 0);
                    }
                    else
                    {
                        var gameStat = parsedResponse.Response.Games.SingleOrDefault(g => g.AppId == _steamOptions.AppId);
                        if (gameStat != null)
                        {
                            results.Add(steamId, gameStat.PlaytimeForeverMinutes);
                        }
                        else
                        {
                            results.Add(steamId, 0);
                        }
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Gets the user profile from the Steam API.
        /// </summary>
        public async Task<SteamProfileExtDTO> GetSteamProfileExternalAsync(ulong steamId)
        {            
            var key = _steamOptions.Key;
            var playerInfoUrl = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={key}&steamids={steamId}";

            var httpClient = _httpClientFactory.CreateClient();
            var steamInfoResponse = await httpClient.GetAsync(playerInfoUrl);

            if (!steamInfoResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Unsuccessful request towards the Steam API: https://api.steampowered.com/ISteamUser/GetPlayerSummaries/");
            }
            
            var response = await steamInfoResponse.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<SteamExtDTO>(response);

            return parsedResponse.Response.Players.FirstOrDefault();
        }

        /// <summary>
        /// Gets the users profiles from the Steam API.
        /// </summary>
        public async Task<IEnumerable<SteamProfileExtDTO>> GetSteamProfilesExternalAsync(IEnumerable<ulong> steamIds)
        {            
            var key = _steamOptions.Key;
            var joinedIds = String.Join(',', steamIds);
            var playerInfoUrl = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={key}&steamids={joinedIds}";

            var httpClient = _httpClientFactory.CreateClient();
            var steamInfoResponse = await httpClient.GetAsync(playerInfoUrl);

            if (!steamInfoResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Unsuccessful request towards the Steam API: https://api.steampowered.com/ISteamUser/GetPlayerSummaries/");
            }

            var response = await steamInfoResponse.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<SteamExtDTO>(response);

            return parsedResponse.Response.Players;
        }

        private void SetMaxConcurrency(string url, int maxConcurrentRequests)
        {
            ServicePointManager.FindServicePoint(new Uri(url)).ConnectionLimit = maxConcurrentRequests;
        }
    }
}
