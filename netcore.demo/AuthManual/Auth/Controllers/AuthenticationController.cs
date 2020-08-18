using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    public class AuthenticationController : Controller
    {
        public ContentResult Index()
        {
            return new ContentResult() { 
             Content= "this is index page",
              StatusCode=200,
               ContentType="text/json"
            };
        }

        public async Task<IActionResult> Login(string username,string password)
        {
            if("admin".Equals(username, StringComparison.CurrentCultureIgnoreCase)&& password.Equals("123456"))
            {
                var claimIdentity = new ClaimsIdentity("Custom");          //身份
                claimIdentity.AddClaim(new Claim(ClaimTypes.Name, username));    //添加信息
                claimIdentity.AddClaim(new Claim(ClaimTypes.Email, "164910441@qq.com"));
                await base.HttpContext.SignInAsync("CustomScheme", new ClaimsPrincipal(claimIdentity), new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                });    //多个身份
                return new JsonResult(new { 
                Result = true,
                Message = "登陆成功"
                });
            }
            else
            {
                await Task.CompletedTask;
                return new JsonResult(new
                {
                    Result = true,
                    Message = "登录失败"
                });
            }
        }
    
        public async Task<IActionResult> Logout()
        {
            await base.HttpContext.SignOutAsync("CustomScheme");
            return new JsonResult(new { 
            Result=true,
            Message="登出成功"
            });
        }

        public async Task<IActionResult> Authentication()
        {
            var result = await base.HttpContext.AuthenticateAsync("CustomScheme");
            if (result?.Principal != null)
            {
                base.HttpContext.User = result.Principal;
                return new JsonResult(new {
                    Result = true,
                    Message = $"认证成功,包含用户{base.HttpContext.User.Identity.Name}"
                });
            }
            else
            {
                return new JsonResult(new
                {
                    Result = true,
                    Message = $"认证失败，用户未登录"
                });
            }
        }

        public async Task<IActionResult> Authorization()
        {
            //认证
            var result = await base.HttpContext.AuthenticateAsync("CustomScheme");
            if (result?.Principal == null)
            {
                return new JsonResult(new
                {
                    Result = true,
                    Message = $"认证失败，用户未登录"
                });
            }
            else
            {
                base.HttpContext.User = result.Principal;
            }
            //授权
            var user = base.HttpContext.User;

            if(user?.Identity?.IsAuthenticated ?? false)
            {
                if (!user.Identity.Name.Equals("admin", StringComparison.CurrentCultureIgnoreCase))
                {
                    await base.HttpContext.ForbidAsync("CustomScheme");
                    return new JsonResult(new
                    {
                        Result = false,
                        Message = $"授权失败，用户{base.HttpContext.User.Identity.Name}没有权限"
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        Result = true,
                        Message = $"授权成功，用户{base.HttpContext.User.Identity.Name}有权限"
                    });
                }
            }
            else
            {
                await base.HttpContext.ChallengeAsync("CustomScheme");
                return new JsonResult(new
                {
                    Result = false,
                    Message = $"授权失败,没有登陆"
                });
            }
        }
    
        public async Task<IActionResult> Info()
        {
            var result = await base.HttpContext.AuthenticateAsync("CustomScheme");
            if(result?.Principal == null)
            {
                return new JsonResult(new
                {
                    Result = true,
                    Message = $"认证失败，用户未登录"
                });
            }
            else
            {
                base.HttpContext.User = result.Principal;


            }
            var user
                 = base.HttpContext.User;
            if (user?.Identity?.IsAuthenticated ?? false)
            {
                if (!user.Identity.Name.Equals("admin", StringComparison.CurrentCultureIgnoreCase))
                {
                    await base.HttpContext.ForbidAsync("CustomScheme");
                    return new JsonResult(new
                    {
                        Result = false,
                        Message = $"授权失败，用户{base.HttpContext.User.Identity.Name}没有权限"
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        Result = true,
                        Message = $"授权成功，正常访问页面",
                        Html="Hello Root!"
                    });
                }
            }
            else
            {
                await base.HttpContext.ChallengeAsync("CustomScheme");
                return new JsonResult(new
                {
                    Result = false,
                    Message = $"授权失败,没有登陆"
                });
            }
        }
    }
}
