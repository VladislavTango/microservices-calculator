using Microsoft.AspNetCore.Components;
using PraktikaMauiBlazor.Intarface;
using PraktikaMauiBlazor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PraktikaMauiBlazor.Components.Pages
{
    public partial class Calculator : ComponentBase
    {
        InputModel item = new InputModel();
        public List<InputModel> items = new List<InputModel>();
        [Parameter]
        public List<InputModel> Items
        {
            get { return items; }
            set { items = value; }
        }
        [Parameter]
        public EventCallback<List<InputModel>> ItemsChanged { get; set; }
        void AddItem(string str)
        {
            item.Text += str;
            if (str == "+" || str == "-" || str == "*" || str == "/")
            {
                var tempItem = new InputModel()
                {
                    Text = item.Text,
                    Currency = item.Currency,
                };
                items.Add(tempItem);
                item.Text = string.Empty;
                ChangeValue(items);
            }
        }
        async Task ChangeValue(List<InputModel> e)
        {
            items = e;
            await ItemsChanged.InvokeAsync(Items);
        }
    }
}
