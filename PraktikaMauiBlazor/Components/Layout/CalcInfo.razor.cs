using Microsoft.AspNetCore.Components;
using PraktikaMauiBlazor.Intarface;
using PraktikaMauiBlazor.Models;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace PraktikaMauiBlazor.Components.Layout
{
    public partial class CalcInfo : ComponentBase
    {
        private string CalculateUrl = "http://localhost:7185/Calcuate";

        public CurrencyNames ChoosenCurrency;

        public string? inputValue;

        [Parameter]
        public List<InputModel> Items
        {
            get { return items; }
            set { items = value; }
        }

        private List<InputModel> items = new List<InputModel>();

        [Parameter]
        public EventCallback<List<InputModel>> ItemsChanged { get; set; }

        [Parameter]
        public string InputValue
        {
            get { return inputValue; }
            set { inputValue = value; }
        }

        [Parameter]
        public EventCallback<string> InputValueChanged { get; set; }

        string CheckFormat(InputModel item)
        {
            Regex regex = new Regex(@"^(\d+)(,\d{2})?$"); ;

            item.Valid = regex.IsMatch(item.Text) && item.Text.Length < 20 ? "valid" : "invalid";

            return item.Valid;
        }

        void AddItem()
        {
            var item = new InputModel()
            {
                Text = string.Empty,
                Currency = CurrencyNames.NON,
                Operation = "+"
            };
            Items.Add(item);
            ChangeValue(Items);
        }

        async Task ChangeValue(List<InputModel> value)
        {
            items = value;
            await ItemsChanged.InvokeAsync(Items);
        }

        async Task ChangeValue(string value)
        {
            inputValue = value;
            await InputValueChanged.InvokeAsync(InputValue);
        }

        void DeleteItem(InputModel item) { 
            Items.Remove(item);
            ChangeValue(Items);

        }

        async Task Calculate() { 
            HttpClient client = new HttpClient();
            var json = new StringContent(
            JsonSerializer.Serialize(Items), 
            Encoding.UTF8, 
            "application/json");

            string? name = await sessionStorage.GetItemAsStringAsync("Name");

            if (!string.IsNullOrEmpty(name)) name = name.Trim('\"');

            var response = await client.PostAsync($"{CalculateUrl}?currency={ChoosenCurrency}&name={name}", json);
            
            inputValue = await response.Content.ReadAsStringAsync();

            ChangeValue(inputValue + $"({Enum.GetName(ChoosenCurrency)})");
        }
    }
}
