using Microsoft.AspNetCore.Mvc;
using My.Project.Ioc.IBusiness;

namespace My.Project.Ioc.WebApi.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ITest _test;
    public WeatherForecastController(ILogger<WeatherForecastController> logger,ITest test)
    {
        _logger = logger;
        _test = test;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet]
    [Route("HW")]
    public string HelloWorld()
    {
        return _test.GetValue();
    }
}
