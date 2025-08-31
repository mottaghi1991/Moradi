using Core.Extention;
using Core.Interface.Admin;
using Domain;
using Domain.User.Permission;
using Fuel_Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace PersonalSite.MiddleWare
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class PermissionCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PermissionCheckMiddleware> _logger;
        public PermissionCheckMiddleware(RequestDelegate next, ILogger<PermissionCheckMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var endpoint = httpContext.GetEndpoint();
            if (endpoint == null)
            {
                await _next(httpContext);
                return;
            }

            // اگر روی اکشن یا کنترلر صفت [AllowAnonymous] زده شده بود، عبور کن
            var allowAnonymous = endpoint.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>();
            if (allowAnonymous != null)
            {
                await _next(httpContext);
               return ;

            }
            var User=httpContext.User;
            if(!User.Identity.IsAuthenticated)
            {
                _logger.LogWarning(EventIdList.Error,
                    "⚠️ کاربر ناشناس تلاش دسترسی داشت -> Path={Path}, Method={Method}, IP={IP}",
                    httpContext.Request.Path,
                    httpContext.Request.Method,
                    httpContext.Connection.RemoteIpAddress?.ToString()
                ); httpContext.Response.Redirect("/SmsLogin");
                return ;
            }
         
            var area = httpContext.GetRouteValue("area")?.ToString();
            var controller = httpContext.GetRouteValue("controller")?.ToString();
            var action = httpContext.GetRouteValue("action")?.ToString();
            var permissionChecker = httpContext.RequestServices.GetRequiredService<Core.Interface.Admin.IPermisionList>();
            
            bool hasAccess =await permissionChecker.HasAccessAsync(httpContext, area, controller, action);


            if (hasAccess)
            {
                await _next(httpContext);
                return ;    

            }
            else
            {

                _logger.LogWarning(EventIdList.Error,
        "⚠️ کاربر {UserId} ({UserName}) دسترسی غیرمجاز -> Area={Area}, Controller={Controller}, Action={Action}, Path={Path}",
        User.GetUserId(),
        User.Identity.Name,
        area ?? "-",
        controller ?? "-",
        action ?? "-",
        httpContext.Request.Path
    );
                httpContext.Response.Redirect("/AccessDenied");
              
                return ;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class PermissionCheckMiddlewareExtensions
    {
        public static IApplicationBuilder UsePermissionCheckMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PermissionCheckMiddleware>();
        }
    }
}
