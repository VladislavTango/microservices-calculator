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
        public CurrencyNames ChoosenCurency;
        public List<InputModel> items = new List<InputModel>();
        public string? inputValue;
       [Parameter]
        public List<InputModel> Items
        {
            get { return items; }
            set { items = value; }
        }
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
            Regex regex;
                regex = new Regex(@"^(\d+)(,\d{2})?$");

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
        async Task ChangeValue(List<InputModel> e)
        {
            items = e;
            await ItemsChanged.InvokeAsync(Items);
        }
        async Task ChangeValue(string e)
        {
            inputValue = e;
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
            name = name!=null? name.Trim('\"'):"null";

            var response = await client.PostAsync($"https://localhost:7185/Calcuate?currency={ChoosenCurency}&name={name}", json);
            
            inputValue = await response.Content.ReadAsStringAsync();
            ChangeValue(inputValue + $"({Enum.GetName(ChoosenCurency)})");
        }
    }
}
