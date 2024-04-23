using HistoryService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HistoryService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly DbService dbService;
        public HistoryController(DbService dbService)
        {
            this.dbService = dbService;
        }
        [HttpGet("SaveData")]
        public IActionResult SaveData(string name,string str)
        {
            dbService.SaveData(name, str);
            return Ok();
        }
        [HttpGet("GetData")]
        public List<string> GetData(string name)
        {
            return dbService.GetAll(name);
        }
    }
}
