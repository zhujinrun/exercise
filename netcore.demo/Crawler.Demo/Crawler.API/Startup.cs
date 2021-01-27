using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Crawler.API.Filter;
using Crawler.API;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using Crawler.Utility.HttpHelper;
using Crawler.API.Services;
using Crawler.API.Scheduler;
using Crawler.API.Interface;
using Crawler.API.Controllers;
using Crawler.API.Job;
using Crawler.API.Model;

namespace Crawler.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookieInfoOptions>(Configuration.GetSection("CookieInfoOptions"));

            services.Configure<MongoDatabaseSettings>(Configuration.GetSection(nameof(MongoDatabaseSettings)));
            services.AddSingleton<IMongoDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);
            services.AddSingleton<TaskSchedulers>();
            services.AddSingleton<PostDataService>();
            services.AddSingleton<WebUtils>();
            services.AddSingleton<GetKolPost>();
            services.AddControllersWithViews().AddNewtonsoftJson(options => { options.UseMemberCasing(); options.UseCamelCasing(true); });  //json格式美化

            services.AddControllers(options=> {
                options.Filters.Add<ApiExceptionFilterAttribute>();
            }).AddNewtonsoftJson();
            services.AddSwaggerGen(s=> {
                s.SwaggerDoc("V1", new OpenApiInfo() { 
                 Title="crawler",
                 Version="version-01",
                 Description="crawler test"
                });
            });
            //services.AddScoped<ApiExceptionFilterAttribute>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddLog4Net();
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(s=>{

                s.SwaggerEndpoint("/swagger/V1/swagger.json", "crawler");
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>

            {
                endpoints.MapControllers();
            });
        }
    }
}
