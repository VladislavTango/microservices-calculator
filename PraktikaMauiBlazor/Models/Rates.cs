using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PraktikaMauiBlazor.Models
{
    public class Rates
    {
        public string? Cur_ID { get; set; }
        public string? Date { get; set; }
        public string? Cur_Abbreviation { get; set; }
        public string? Cur_Scale { get; set; }
        public string? Cur_Name { get; set; }
        public double? Cur_OfficialRate { get; set; }
    }
}
