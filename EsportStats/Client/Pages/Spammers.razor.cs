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

        protected override async Task OnInitializedAsync()
        {
            entries = await _http.GetFromJsonAsync<IEnumerable<TopListEntryDTO>>("/Api/Spammers");
            isLoaded = true;
        }
    }
}
