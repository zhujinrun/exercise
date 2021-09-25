using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Hosting;

namespace Signalr.Server
{
    class Program
    {
        static void Main(string[] args)
        {
           
            Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((context,config)=> {
                config.SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            }).ConfigureWebHostDefaults(builder=>{
                builder.UseStartup<Startup>();
            }).Build().Run();
        }
    }
}
