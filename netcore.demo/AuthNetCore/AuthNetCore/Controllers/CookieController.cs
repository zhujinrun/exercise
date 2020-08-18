using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthNetCore.Controllers
{
    /// <summary>
    /// netcore自带授权 鉴权模式
    /// </summary>
    [Authorize]
    public class CookieController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username,string password)
        {
            if ("admin".Equals(username, StringComparison.CurrentCultureIgnoreCase) && password.Equals("123456"))
            {
                var claimIdentity = new ClaimsIdentity("Cookie", JwtClaimTypes.Name, JwtClaimTypes.Role);
                claimIdentity.AddClaim(new Claim(JwtClaimTypes.Name, username));
                claimIdentity.AddClaim(new Claim(JwtClaimTypes.Email, "164910441@qq.com"));

                await base.HttpContext.SignInAsync(new ClaimsPrincipal(claimIdentity), new AuthenticationProperties {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                });
                return new JsonResult(new { 
                Result=true,
                Message = "登陆成功"
                });
            }
            else
            {
                await Task.CompletedTask;
                return new JsonResult(new
                {
                    Result = false,
                    Message = "登陆失败"
                });
            }
        }
        
        public async Task<IActionResult> AuthenticationAuthorization()
        {
            var result = await HttpContext.AuthenticateAsync();
            if(result?.Principal == null)
            {
                return new JsonResult(new
                {
                    Result = true,
                    Message = "认证失败,用户未登录"
                });
            }
            else
            {
                HttpContext.User = result.Principal;
            }

            var user = HttpContext.User;
            if (user?.Identity?.IsAuthenticated ?? false)
            {
                if(!user.Identity.Name.Equals("admin", StringComparison.CurrentCultureIgnoreCase))
                {
                    await HttpContext.ForbidAsync();
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
                        Result = false,
                        Message = $"授权成功，用户{base.HttpContext.User.Identity.Name}有权限"
                    });

                }
               
            }
            else
            {
                await HttpContext.ChallengeAsync();
                return new JsonResult(new
                {
                    Result = false,
                    Message = $"授权失败"
                });
            }
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return new JsonResult(new
            {
                Result = true,
                Message = "登出成功"
            });
        }
        public IActionResult Info()
        {
            return View();
        }
    }
}
