using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthNetCore.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthNetCore
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
            services.AddControllersWithViews().AddNewtonsoftJson();

            #region 扩展cookie
            services.AddScoped<ITicketStore, MemoryCacheTicketStore>();
            services.AddMemoryCache(); 
            #endregion

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "Cookie/Login";
                
            }).AddCookie(
                options =>
                {
                    options.Events = new CookieAuthenticationEvents()
                    {
                        OnSignedIn = async context => {

                            Console.WriteLine($"{context.Request.Path} is OnSignedIn");
                            await Task.CompletedTask;
                        },
                        OnSigningIn = async context => {

                            Console.WriteLine($"{context.Request.Path} is OnSigningIn");
                            await Task.CompletedTask;
                        },
                        OnSigningOut = async context =>
                          {
                              Console.WriteLine($"{context.Request.Path} is OnSigningOut");
                            await Task.CompletedTask;
                          }

                    }; //扩展事件

                }
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //CookieAuthentication
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
