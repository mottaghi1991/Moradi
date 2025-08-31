using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Core.Enums;
using Core.Extention;
using Domain;

namespace PersonalSite.MiddleWare
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SessionLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _factory;
        public SessionLogMiddleware(RequestDelegate next, ILoggerFactory factory)
        {
            _next = next;
            _factory = factory.CreateLogger("Session");
        }

        public Task Invoke(HttpContext httpContext)
        {
            var path=httpContext.Request.Path;
            var sessionId = httpContext.Session.Id;
            var a = httpContext.User.GetUserId();
            if (!string.IsNullOrEmpty(sessionId))
            {
                _factory.LogInformation(EventIdList.Login, "User Requset path={path} SessionId={SessionId} start",path,sessionId);
            }

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SessionLogMiddlewareExtensions
    {
        public static IApplicationBuilder UseSessionLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionLogMiddleware>();
        }
    }
}
