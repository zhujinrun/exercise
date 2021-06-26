using Exceptionless;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        public CommonTool.Service.ILogger Logger { get; }

        public WeatherForecastController(ILogger<WeatherForecastController> logger, CommonTool.Service.ILogger loggerex)
        {
            _logger = logger;
            Logger = loggerex;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            _logger.LogInformation("_logger 原始");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet]
        [Route("Gets")]
        public async Task<string> Gets()
        {
            try
            {
                throw new Exception();
            }
            catch(Exception ex)
            {
                ex.ToExceptionless().Submit();
            }
            _logger.LogInformation(string.Format("Gets方法调用了"));
            _logger.LogWarning(string.Format("Gets方法调用了"));
            _logger.LogError(string.Format("Gets方法调用了"));
            _logger.LogDebug(string.Format("Gets方法调用了"));

            Logger.Trace($"User Login Successfully. Time:{DateTime.Now.ToString()}", "Tag1", "Tag2");
            Logger.Debug($"User Login Successfully. Time:{DateTime.Now.ToString()}", "Tag1", "Tag2");
            Logger.Info($"User Login Successfully. Time:{DateTime.Now.ToString()}", "Tag1", "Tag2");
            Logger.Warn($"User Login Successfully. Time:{DateTime.Now.ToString()}", "Tag1", "Tag2");
            Logger.Error($"User Login Successfully. Time:{DateTime.Now.ToString()}", "Tag1", "Tag2");
            return "ok";
        }
    }
}
