using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crawler.API.Scheduler;
using Crawler.API.Services;
using Crawler.Utility.HttpHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Crawler.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KolPostController : Controller
    {
        private readonly PostDataService _postDataService;
        private readonly WebUtils _webUtils;
        private readonly IConfiguration _configuration;
        private readonly TaskSchedulers _taskSchedulers;

        public KolPostController(PostDataService postDataService, IConfiguration configuration,  WebUtils webUtils, TaskSchedulers taskSchedulers)
        {
            _postDataService = postDataService;
            _configuration = configuration;
            _webUtils = webUtils;
            _taskSchedulers = taskSchedulers;
        }
        [Route("Index")]
        [HttpGet]
        public async Task Index(string url = "https://www.instagram.com/p/{0}/?__a=1")
        {
            await _taskSchedulers.CraKolPost(_configuration["Cookie"], _configuration["Cookie2"], url, _postDataService, _webUtils);

        }
        [Route("GetProgile")]
        [HttpGet]
        public async Task GetProgile(string url = "https://www.instagram.com/{0}/?__a=1")
        {
            await _taskSchedulers.CraKolPost(_configuration["Cookie"], _configuration["Cookie2"], url, _postDataService, _webUtils);

        }
    }
}
