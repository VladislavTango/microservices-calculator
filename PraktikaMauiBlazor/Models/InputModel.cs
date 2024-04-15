using PraktikaMauiBlazor.Intarface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PraktikaMauiBlazor.Models
{
    public class InputModel
    {
        public string? Text { get; set; }
        public CurrencyNames Currency { get; set; }
        public string? Operation  { get; set; }
        public string? Valid = "invalid";
    }
}
