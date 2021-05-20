using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EsportStats.Shared.DTO;
using EsportStats.Shared.Enums;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;

namespace EsportStats.Client.Pages
{

    public class HeroSelection
    {
        public Hero SelectedHero { get; set; }
        public Hero CurrentlyDisplayed { get; set; }
    }

    public partial class HeroStats : ComponentBase
    {
        [Inject]
        private HttpClient _http { get; set; }

        HeroSelection selection = new HeroSelection() { 
            SelectedHero = Hero.PleaseSelect,
            CurrentlyDisplayed = Hero.PleaseSelect
        };

        IEnumerable<TopListEntryDTO> entries = new List<TopListEntryDTO>();
        bool isLoading = false;
        bool isError = false;
        bool isServiceDown = false;

        private async Task HandleChanges()
        {
            try
            {
                isLoading = true;
                var route = $"/Api/Heroes/{(int)selection.SelectedHero}";

                var response = await _http.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<IEnumerable<TopListEntryDTO>>();

                    entries = result;
                    selection.CurrentlyDisplayed = selection.SelectedHero;
                    isLoading = false;
                    isServiceDown = false;
                    isError = false;
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
                    isLoading = false;
                    entries = new List<TopListEntryDTO>();                    
                }
            }
            catch (HttpRequestException ex)
            {
                entries = new List<TopListEntryDTO>();                
                isLoading = false;
                isError = true;
            }
        }
    }

}
