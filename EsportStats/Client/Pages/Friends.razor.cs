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

        // TODO: get from steam account
        string username = "Username";
        string userimage = "http://placehold.it/160x160";
        IEnumerable<SteamFriendDTO> friends = new List<SteamFriendDTO>();    

        protected override async Task OnInitializedAsync()
        {
            try
            {
                friends = await _http.GetFromJsonAsync<IEnumerable<SteamFriendDTO>>("/Api/Friends");
            }
            catch (HttpRequestException e)
            {
                // TODO: Logging/Error message
                friends = new List<SteamFriendDTO>();
            }
        }




    }

}
