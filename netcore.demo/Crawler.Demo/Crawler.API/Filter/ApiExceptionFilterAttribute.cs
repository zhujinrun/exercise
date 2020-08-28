using Crawler.API.Controllers;
using log4net.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.API.Filter
{
    public class ApiExceptionFilterAttribute: ExceptionFilterAttribute
    {
        private readonly ILogger<ApiExceptionFilterAttribute> _logger;
        public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            if(context.Exception !=null & !context.ExceptionHandled)
            {
                var errMessage = $"This is {context.HttpContext.Request.RouteValues["controller"]} ->{context.HttpContext.Request.RouteValues["action"]} Error,Message: {context.Exception.Message}";
                _logger.LogError(errMessage);
                Console.WriteLine(errMessage);
               
                context.ExceptionHandled = true;
            }
        }
    }
}
