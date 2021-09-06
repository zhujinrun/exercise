using AspNetCoreMidddleware.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspNetCoreMidddleware.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [ResponseCache(Duration = 600)]    //中间件跨浏览器 缓存第三步  只有ie浏览器会缓存
    public IActionResult InfoCache()
    {
        base.ViewBag.Now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
        return View();
    }
}
