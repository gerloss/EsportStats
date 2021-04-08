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

        private async Task HandleChanges()
        {
            try
            {
                isLoading = true;
                var route = $"/Api/Heroes/{(int)selection.SelectedHero}";
                var result = await _http.GetFromJsonAsync<IEnumerable<TopListEntryDTO>>(route);
                entries = result;
                selection.CurrentlyDisplayed = selection.SelectedHero;
                isLoading = false;
            }
            catch (HttpRequestException ex)
            {
                // TODO: Logging/Error message
                entries = new List<TopListEntryDTO>();
                selection.CurrentlyDisplayed = Hero.PleaseSelect;
                isLoading = false;
            }
        }
    }

}
