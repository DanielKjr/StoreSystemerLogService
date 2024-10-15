using LogService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace LogService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogController : ControllerBase
    {
        [HttpGet]
        [Route("getTransactionLog")]
        public async Task<ActionResult<bool>> GetTransactionLog()
        {
            return Ok(true);
        }
    }
}
