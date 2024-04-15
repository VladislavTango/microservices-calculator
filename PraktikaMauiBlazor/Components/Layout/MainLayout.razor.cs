using PraktikaMauiBlazor.Intarface;
using PraktikaMauiBlazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PraktikaMauiBlazor.Components.Layout
{
    public partial class MainLayout
    {
        UserResponse userResponse = new();
        string Name;
        string Password;
        string? NameFromStorage = null;
        private string inputValue;
        string? InputValue {
            get {
                return inputValue;
                    
            }
            set
            {
                inputValue = value;
                OnItemChanged();
            }
        }
        private List<InputModel> Items = new List<InputModel>();
        private List<string> CalcHistory = new List<string>();
        protected override async Task OnInitializedAsync()
        {
            HttpClient client = new HttpClient();         
        }

        async Task OnItemChanged()
        {
            HttpClient client = new HttpClient();
            if (NameFromStorage != null)
            {
                var response = client.GetFromJsonAsync<List<string>>($"http://localhost:7002/History/GetData?name={NameFromStorage}");
                CalcHistory = response.Result;
            }
            StateHasChanged();
        }
        async Task RegAsync()
        {
            if (Name.Length > 5 && Password.Length > 5)
            {
                HttpClient client = new HttpClient();
                userResponse = client.GetFromJsonAsync<UserResponse>($"http://localhost:7178/Auth/Reg?name={Name}&password={Password}").Result;
                await sessionStorage.SetItemAsync("Name", userResponse.username);
                await sessionStorage.SetItemAsync("Token", userResponse.access_token);
                NameFromStorage = userResponse.username;
                OnItemChanged();
            }
        }

        async Task LogAsync(){
            if (Name.Length > 5 && Password.Length > 5)
            {
                HttpClient client = new HttpClient();
                userResponse = client.GetFromJsonAsync<UserResponse>($"https://localhost:7178/Auth/Log?name={Name}&password={Password}").Result;
                await sessionStorage.SetItemAsync("Name", userResponse.username);
                await sessionStorage.SetItemAsync("Token", userResponse.access_token);
                NameFromStorage = userResponse.username;
                OnItemChanged();
            }
        }
    }
}
