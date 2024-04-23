using PraktikaMauiBlazor.Models;
using System.Net.Http.Json;

namespace PraktikaMauiBlazor.Components.Layout
{
    public partial class MainLayout
    {
        private UserResponse userResponse = new();

        private string Name;

        private string Password;

        private string? NameFromStorage = null;

        private string inputValue;

        private string AuthUrl = "http://localhost:7178/Auth";

        private string HistoryUrl = "http://localhost:7002/History";

        private string? InputValue {
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

        private async Task OnItemChanged()
        {
            HttpClient client = new HttpClient();
            if (NameFromStorage != null)
            {
                var response = client.GetFromJsonAsync<List<string>>($"{HistoryUrl}/GetData?name={NameFromStorage}");
                CalcHistory = response.Result;
            }
            StateHasChanged();
        }

        private async Task RegAsync()
        {
            if (Name.Length > 5 && Password.Length > 5)
            {
                HttpClient client = new HttpClient();
                userResponse = client.GetFromJsonAsync<UserResponse>($"{AuthUrl}/Reg?name={Name}&password={Password}").Result;
                await sessionStorage.SetItemAsync("Name", userResponse.username);
                await sessionStorage.SetItemAsync("Token", userResponse.access_token);
                NameFromStorage = userResponse.username;
                OnItemChanged();
            }
        }

        private async Task LogAsync(){
            if (Name.Length > 5 && Password.Length > 5)
            {
                HttpClient client = new HttpClient();
                userResponse = client.GetFromJsonAsync<UserResponse>($"{AuthUrl}/Log?name={Name}&password={Password}").Result;
                await sessionStorage.SetItemAsync("Name", userResponse.username);
                await sessionStorage.SetItemAsync("Token", userResponse.access_token);
                NameFromStorage = userResponse.username;
                OnItemChanged();
            }
        }
    }
}
