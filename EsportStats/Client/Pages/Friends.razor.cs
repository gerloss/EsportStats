using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EsportStats.Shared.DTO;
using EsportStats.Shared.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace EsportStats.Client.Pages
{

    public partial class Friends : ComponentBase
    {
        [Inject]
        private HttpClient _http { get; set; }
        
        IEnumerable<SteamUserDTO> friends = new List<SteamUserDTO>();
        bool isLoaded = false;

        protected override async Task OnInitializedAsync()
        {            
            friends = await _http.GetFromJsonAsync<IEnumerable<SteamUserDTO>>("/Api/Friends");
            isLoaded = true;
        }
    }

}
