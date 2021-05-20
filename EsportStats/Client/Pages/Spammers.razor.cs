using EsportStats.Shared.DTO;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EsportStats.Client.Pages
{
    public partial class Spammers : ComponentBase
    {
        [Inject]
        private HttpClient _http { get; set; }

        IEnumerable<TopListEntryDTO> entries = new List<TopListEntryDTO>();
        bool isLoaded = false;
        bool isError = false;
        bool isServiceDown = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var route = "/Api/Spammers";
                var response = await _http.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    entries = await response.Content.ReadFromJsonAsync<IEnumerable<TopListEntryDTO>>();
                    isServiceDown = false;
                    isError = false;
                    isLoaded = true;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        isServiceDown = true;
                    }
                    else
                    {
                        isError = true;
                    }                    
                    entries = new List<TopListEntryDTO>();
                    isLoaded = true;
                }
            } catch (HttpRequestException ex)
            {
                entries = new List<TopListEntryDTO>();                
                isError = true;
                isLoaded = true;
            }

        }
    }
}
