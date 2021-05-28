using EsportStats.Server.Common;
using EsportStats.Server.Data;
using EsportStats.Server.Data.Entities;
using EsportStats.Shared.Enums;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EsportStats.Shared.Common;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace EsportStats.Server.Services
{
    public interface IOpenDotaService
    {
        public Task<IEnumerable<HeroStatDTO>> GetHeroStatsAsync(IDotaPlayer player);
        public Task<IEnumerable<HeroStatDTO>> GetHeroStatsAsync(IEnumerable<IDotaPlayer> players);
        public Task<IEnumerable<TopListEntryExtDTO>> GetTopListEntriesAsync(ulong steamId, Metric metric);
    }

    public class OpenDotaService : IOpenDotaService
    {        
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly OpenDotaOptions _openDotaOptions;

        public OpenDotaService(
            IHttpClientFactory httpClientFactory,
            OpenDotaOptions openDotaOptions)
        {            
            _httpClientFactory = httpClientFactory;            
            _openDotaOptions = openDotaOptions;
        }

        /// <summary>
        /// Gets the hero statistics of a player
        /// </summary>    
        public async Task<IEnumerable<HeroStatDTO>> GetHeroStatsAsync(IDotaPlayer player)
        {
            //OpenDota api uses the 32 bit steam ids so a conversion must be done
            var steamId32 = player.SteamId.ToSteam32();

            var heroStatsUrl = $"https://api.opendota.com/api/players/{steamId32}/heroes";

            var httpClient = _httpClientFactory.CreateClient();
            var heroStatsResponse = await httpClient.GetAsync(heroStatsUrl + "?api_key=" + _openDotaOptions.Key);

            if (heroStatsResponse.IsSuccessStatusCode)
            {
                var response = await heroStatsResponse.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<List<HeroStatDTO>>(response);
                foreach (var stat in parsedResponse)
                {
                    stat.User = player; // so we know whose stats these are in case of requesting stats for multiple users in parallel
                }

                return parsedResponse;
            }
            else if (heroStatsResponse.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                throw new TooManyRequestsException("OpenDota API is overloaded.");
            }
            else
            {
                throw new HttpRequestException("Unsuccessful request towards the OpenDota API: " + heroStatsUrl);
            }               
        }

        public async Task<IEnumerable<HeroStatDTO>> GetHeroStatsAsync(IEnumerable<IDotaPlayer> players)
        {
            var heroStatsBaseUrl = $"https://api.opendota.com/api/players/";
            var httpClient = _httpClientFactory.CreateClient();
            SetMaxConcurrency(heroStatsBaseUrl, _openDotaOptions.BatchSize);

            var urls = players.Select(player => heroStatsBaseUrl
                + player.SteamId.ToSteam32() 
                + "/heroes?api_key=" + _openDotaOptions.Key
            );

            var numberOfBatches = (int)Math.Ceiling((double)urls.Count() / _openDotaOptions.BatchSize); // run the parallel requests in batches            
            var results = new List<HeroStatDTO>();

            for (int i = 0; i < numberOfBatches; i++)
            {
                var batchOfUrls = urls.Skip(i * _openDotaOptions.BatchSize).Take(_openDotaOptions.BatchSize);
                var batchOfRequests = batchOfUrls.Select(url => httpClient.GetAsync(url));
                var httpResponses = await Task.WhenAll(batchOfRequests);

                foreach (var response in httpResponses)
                {                                                                                                   //  [0]   [1]      [2]         [3]             [4]
                    int steamId32 = Int32.Parse(response.RequestMessage.RequestUri.Segments[3].Split("/").First()); // {"/",  "api/",  "players/", "{steamId32}/", "heroes/"}

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var parsedResponse = JsonConvert.DeserializeObject<List<HeroStatDTO>>(responseString);
                        foreach (var stat in parsedResponse)
                        {
                            // so we know whose stats these are in case of requesting stats for multiple users in parallel
                            stat.User = players.SingleOrDefault(p => p.SteamId == steamId32.ToSteam64()); 
                        }

                        results.AddRange(parsedResponse);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        throw new TooManyRequestsException("OpenDota API is overloaded.");
                    }
                    else
                    {
                        throw new HttpRequestException("Unsuccessful request towards the OpenDota API: " + response.RequestMessage.RequestUri.AbsolutePath);
                    }
                }
            }

            return results;
        }

        public async Task<IEnumerable<TopListEntryExtDTO>> GetTopListEntriesAsync(ulong steamId, Metric metric)
        {                        
            var entriesUrl = $"https://api.opendota.com/api/players/{steamId.ToSteam32()}/matches?sort={metric.GetShortName()}&limit=10";

            var httpClient = _httpClientFactory.CreateClient();
            
            var heroStatsResponse = await httpClient.GetAsync(entriesUrl + "&api_key=" + _openDotaOptions.Key);

            if (heroStatsResponse.IsSuccessStatusCode)
            {
                var response = await heroStatsResponse.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<List<TopListEntryExtDTO>>(response);

                return parsedResponse;
            } 
            else if (heroStatsResponse.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                throw new TooManyRequestsException("OpenDota API is overloaded.");
            }
            else 
            {
                throw new HttpRequestException("Unsuccessful request towards the OpenDota API: " + entriesUrl);
            }
        }
        private void SetMaxConcurrency(string url, int maxConcurrentRequests)
        {
            ServicePointManager.FindServicePoint(new Uri(url)).ConnectionLimit = maxConcurrentRequests;
        }
    }
}
