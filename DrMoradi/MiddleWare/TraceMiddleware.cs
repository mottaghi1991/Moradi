using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

public class TraceMiddleware
{
    private readonly RequestDelegate _next;

    public TraceMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var id = Guid.NewGuid().ToString("N").Substring(0, 6); // کوتاه برای دیباگ راحت
        var path = context.Request.Path + context.Request.QueryString;

        Console.WriteLine($"[{id}] START: {context.Request.Method} {path}");

        // مرحله قبل از ادامه
        var startStatus = context.Response.StatusCode;

        await _next(context);

        // مرحله بعد از ادامه
        var endStatus = context.Response.StatusCode;
        var endPath = context.Request.Path + context.Request.QueryString;

        if (startStatus != endStatus || path != endPath)
        {
            Console.WriteLine($"[{id}] CHANGE: {startStatus} -> {endStatus}, PATH: {path} -> {endPath}");
        }

        Console.WriteLine($"[{id}] END: Status={endStatus}");
    }
}

// اکستنشن
public static class TraceMiddlewareExtensions
{
    public static IApplicationBuilder UseTraceMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<TraceMiddleware>();
    }
}
