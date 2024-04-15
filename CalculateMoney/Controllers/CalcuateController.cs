using CalculateMoney.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using PraktikaMauiBlazor.Intarface;
using System.Data;
using System.Text;

namespace CalculateMoney.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CalcuateController : ControllerBase
    {
        [HttpPost]
        public IActionResult PostData([FromBody] List<InputModel> data, [FromQuery] CurrencyNames currency,string name)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder HistoryBuilder = new StringBuilder();

            foreach (var item in data)
            {
                HistoryBuilder.Append($"{item.Text}({Enum.GetName(item.Currency)}){item.Operation}");
                item.Text =  ToBYN(item);
                sb.Append(item.Text+item.Operation);
            }
            sb.Remove(sb.Length - 1, 1);
            HistoryBuilder.Remove(HistoryBuilder.Length - 1, 1);
            sb.Replace(",", ".");

            dynamic result = new DataTable().Compute(sb.ToString(), null);
            result = Math.Round(ToChoosen(result.ToString(), currency),2);
            result = result + "";
            HistoryBuilder.AppendFormat("={0}({1}) , {2}", result, Enum.GetName(currency), DateTime.Now.ToString("dd/MM/yyyy"));

            HttpClient client = new HttpClient();
            if(name!="null")
            client.GetStringAsync
                ($"https://localhost:7002/History/SaveData?name={name}&str={HistoryBuilder}");
            return Ok(result+"");
        }
        double ToChoosen(string result, CurrencyNames currency) {
            if (Enum.GetName(currency) == "NON"
                || Enum.GetName(currency) == "BYN")
                return Convert.ToDouble(result);

            HttpClient client = new HttpClient();
            var response = client.GetStringAsync
                ($"https://api.nbrb.by/exrates/rates/{currency.ToString()}?parammode=2");

            var rate = JsonConvert.DeserializeObject<Rates>(response.Result);

            return (Math.Round(Convert.ToDouble(result) /
               Math.Round(rate.Cur_OfficialRate, 2) / rate.Cur_Scale,2));
        }
        string ToBYN(InputModel item) {

            if (Enum.GetName(item.Currency) == "NON"
                || Enum.GetName(item.Currency) == "BYN") 
                return item.Text;

            HttpClient client = new HttpClient();
 
            var response = client.GetStringAsync
                ($"https://api.nbrb.by/exrates/rates/{Enum.GetName(item.Currency)}?parammode=2");

            var rate = JsonConvert.DeserializeObject<Rates>(response.Result);

            return (Convert.ToDouble(item.Text) * 
                Math.Round(rate.Cur_OfficialRate, 2) / rate.Cur_Scale)
                .ToString();
        }
    }
}
