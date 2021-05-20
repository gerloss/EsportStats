using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EsportStats.Shared.DTO;
using EsportStats.Shared.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace EsportStats.Client.Pages
{

    public class MetricSelection
    {
        public Metric Selected { get; set; }
        public Metric CurrentlyDisplayed { get; set; }
    }

    public partial class Lists : ComponentBase
    {
        [Inject]
        private HttpClient _http { get; set; }

        MetricSelection selection = new MetricSelection() 
        { 
            Selected = Metric.PleaseSelect, 
            CurrentlyDisplayed = Metric.PleaseSelect 
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
                var route = $"/Api/Lists/{(int)selection.Selected}";
                var response = await _http.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<IEnumerable<TopListEntryDTO>>();
                    entries = result;
                    selection.CurrentlyDisplayed = selection.Selected;
                    isLoading = false;
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
