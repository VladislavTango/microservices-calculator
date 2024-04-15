using Microsoft.AspNetCore.Components;
using PraktikaMauiBlazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PraktikaMauiBlazor.Components.Pages
{
    public partial class OutputField : ComponentBase
    {
        string? InputValue { get; set; }
        protected override void OnParametersSet()
        {
            StateHasChanged();
        }
    }
}
