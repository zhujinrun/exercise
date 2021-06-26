using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SkyWalkingDemo2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        [HttpGet]
        public async Task<string> Get()
        {
            var client = new HttpClient();
            var result = await client.GetStringAsync("https://localhost:5001/home/index");
            return await client.GetStringAsync("https://localhost:5001/home/index");
        }
    }
}
