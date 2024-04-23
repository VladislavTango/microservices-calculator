using PraktikaMauiBlazor.Intarface;

namespace CalculateMoney.Models
{
    public class InputModel
    {
        public string? Text { get; set; }
        public CurrencyNames Currency { get; set; }
        public string? Operation { get; set; }

        public string Valid = "invalid";
    }
}
