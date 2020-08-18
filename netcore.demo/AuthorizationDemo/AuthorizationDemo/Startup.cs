using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizationDemo.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthorizationDemo
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

            services.AddAuthentication(options=> {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
                options.LoginPath = "/Authorization/Index";
                options.AccessDeniedPath = "/Authorization/Index";
            
            });


            #region 多种策略组合验证
            services.AddAuthorization(options =>
                {
                    options.AddPolicy("AdminPolicy",
                        policyBuilder => policyBuilder
                        .RequireRole("Admin")//Claim的Role是Admin
                        .RequireUserName("admin")//Claim的Name是admin
                        .RequireClaim(ClaimTypes.Email)//必须有某个Cliam        
                        );//内置

                options.AddPolicy("UserPolicy",
                        policyBuilder => policyBuilder.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == ClaimTypes.Role)
                        && context.User.Claims.First(c => c.Type.Equals(ClaimTypes.Role)).Value == "Admin")
                   //.Combine(qqEmailPolicy)
                   );//自定义
                     //policy层面  没有Requirements
                     //自定义扩展规则
                     //options.AddPolicy("QQEmail", policyBuilder => policyBuilder.Requirements.Add(new QQEmailRequirement()));
                    //options.AddPolicy("DoubleEmail", policyBuilder => policyBuilder.Requirements.Add(new DoubleEmailRequirement()));
                    //services.AddSingleton<IAuthorizationHandler, QQMailHandler>();
                }); 
            #endregion
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
            app.UseAuthentication(); //赋予角色权限   跟下面的顺序不能错。如果没有用到role控制权限，这行可以不需要
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
