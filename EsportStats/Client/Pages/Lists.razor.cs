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

        private async Task HandleChanges()
        {            
            try
            {
                var route = $"/Api/Lists/{(int)selection.Selected}";
                var result = await _http.GetFromJsonAsync<IEnumerable<TopListEntryDTO>>(route);
                entries = result;
                selection.CurrentlyDisplayed = selection.Selected;
            }
            catch (HttpRequestException ex)
            {
                // TODO: Logging/Error message
                entries = new List<TopListEntryDTO>();
                selection.CurrentlyDisplayed = Metric.PleaseSelect;
            }
        }


    }

}
