using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthorizationDemo.Utility
{
    public class DoubleEmailRequirement:IAuthorizationRequirement
    {
    }
    public class QQMailHandler : AuthorizationHandler<DoubleEmailRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DoubleEmailRequirement requirement)
        {
            if (context.User != null && context.User.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                var email = context.User.FindFirst(c => c.Type == ClaimTypes.Email).Value;
                if (email.EndsWith("@qq.com", StringComparison.OrdinalIgnoreCase))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    //context.Fail();//不设置失败
                }
            }
            return Task.CompletedTask;
        }
    }
}
