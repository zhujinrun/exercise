using Crawler.Models;
using Crawler.Selenium.Helper;
using Crawler.Service;
using Crawler.Service.Config;
using Crawler.Utility.HttpHelper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;

namespace CrawlerConsole.DiService
{

   
    public static class ServiceDiExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<WebUtils>();
            services.AddScoped<SeleniumHelper>();
            services.AddScoped<CrawlerConsole.token.TokenHelper>();

        }

        public static T GetService<T>()
        {
            if (ServiceManager.TryRegisterService(ServiceDiExtension.AddServices))
            {
                return CustomApplicationService.GetService<T>();
            }
            return default(T);
        }
    }
}
