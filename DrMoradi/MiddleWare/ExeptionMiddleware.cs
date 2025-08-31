using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net;
using Serilog;
using Core.Enums;
using Core.Service.Interface.Users;
using System.Diagnostics;
using System.IO;
using Core.Extention;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace PersonalSite.MiddleWare
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExeptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExeptionMiddleware(RequestDelegate next,ILoggerFactory factory)
        {
            _next = next;
            _logger=factory.CreateLogger("Error");
       
        }

        public async Task InvokeAsync(HttpContext httpContext)
        
        {
          var responsecode=  httpContext.Response.StatusCode;
            try
            {

                await _next(httpContext);
          
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }

        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var user = context.User.GetUserId();
            var path = context.Request.Path;
            var identi = context.TraceIdentifier;
            _logger.LogError(Domain.EventIdList.Error,exception, "(Exceptions)User={UserId} with Ip={Ip} Request={RequestPath} with TraceId={TraceId} get Exception", user, context.Connection.RemoteIpAddress,path, identi);

            context.Response.Redirect("/Error");
            return Task.CompletedTask;
            var response = new { message = "An error occurred while processing your request." };


            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }
    }


    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExeptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExeptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExeptionMiddleware>();
        }
    }
}
