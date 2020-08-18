using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.Remote;
using Standard.FrameWork.SelenuimHelper;

namespace Crawler.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrTaskController : ControllerBase
    {

        public static RemoteWebDriver driver;
        [Route("Login")]
        [HttpGet]
        public void Login(string jsonCookie,string url)
        {
            driver = new SeleOperator().Login(jsonCookie, url);
        }
    }
}
