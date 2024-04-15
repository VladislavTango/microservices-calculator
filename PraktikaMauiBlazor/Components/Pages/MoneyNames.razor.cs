using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using PraktikaMauiBlazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace PraktikaMauiBlazor.Components.Pages
{
    public partial class MoneyNames  : ComponentBase
    {
        List<Rates> rates = new List<Rates>();
        protected override async Task OnInitializedAsync()      
        { 
            HttpClient client = new HttpClient();
            var response = client.GetStringAsync("https://api.nbrb.by/exrates/rates?periodicity=0");
            rates = JsonConvert.DeserializeObject<List<Rates>>(response.Result);
        }
    }
}
