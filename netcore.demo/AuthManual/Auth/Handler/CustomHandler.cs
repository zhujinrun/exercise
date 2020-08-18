using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Auth.Handler
{
    public class CustomHandler : IAuthenticationHandler, IAuthenticationSignInHandler, IAuthenticationSignOutHandler
    {
        public AuthenticationScheme Scheme { get; set; }
        public HttpContext Context { get; set; }
        public async Task<AuthenticateResult> AuthenticateAsync()
        {
            var cookie = Context.Request.Cookies["CustomCookie"];
            if (string.IsNullOrEmpty(cookie))
            {
                await Task.CompletedTask;
                return  AuthenticateResult.NoResult();
            }
            return AuthenticateResult.Success(Deserialize(cookie));
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            //Context.Response.Redirect("/Account/Login");//跳转页面--上端返回json
            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            Context.Response.StatusCode = 403;
            return Task.CompletedTask;
            
        }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            Scheme = scheme;
            Context = context;
            return Task.CompletedTask;
        }

        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            var ticket = new AuthenticationTicket(user, properties, Scheme.Name);  //票据
            Context.Response.Cookies.Append("CustomCookie", Serialize(ticket));
            return Task.CompletedTask;
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            Context.Response.Cookies.Delete("CustomCookie");
            return Task.CompletedTask;
        }

        private AuthenticationTicket Deserialize(string content)
        {
            byte[] byteTicket = System.Text.Encoding.Default.GetBytes(content);
            return TicketSerializer.Default.Deserialize(byteTicket);
        }
        private string Serialize(AuthenticationTicket ticket)
        {

            //需要引入  Microsoft.AspNetCore.Authentication

            byte[] byteTicket = TicketSerializer.Default.Serialize(ticket);
            return Encoding.Default.GetString(byteTicket);
        }
    }
}
