using Core.Extention;
using Core.Service.Interface.Users;
using PersonalSite.MiddleWare;
using System.IO;

namespace DrMoradi.MiddleWare
{
    public class CheckFullNameMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CheckFullNameMiddleware> _logger;
        public CheckFullNameMiddleware(RequestDelegate next, ILogger<CheckFullNameMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            var area = httpContext.GetRouteValue("area")?.ToString();
            var isFillFormPage = httpContext.Request.Path == "/UserPanel/UserPanel/FillForm";

            if (area == "UserPanel" && httpContext.User.Identity.IsAuthenticated && !isFillFormPage)
            {
                // 🍀 چک سشن برای جلوگیری از کوئری غیرضروری
                var fullNameStatus = httpContext.Session.GetString("FullNameFilled");

                if (fullNameStatus != "true") // یعنی یا مقداردهی نشده یا خالیه
                {
                    var inter = httpContext.RequestServices.GetRequiredService<IUser>();
                    var Userinfo = await inter.GetUserByUserId(httpContext.User.GetUserId());

                    if (string.IsNullOrWhiteSpace(Userinfo.FullName))
                    {
                        httpContext.Session.SetString("FullNameFilled", "false");
                        httpContext.Response.Redirect("/UserPanel/UserPanel/FillForm");
                        return;
                    }
                    else
                    {
                        httpContext.Session.SetString("FullNameFilled", "true");
                    }
                }
                else if (fullNameStatus == "false")
                {
                    httpContext.Response.Redirect("/UserPanel/UserPanel/FillForm");
                    return;
                }
            }

            await _next(httpContext);
            return;
        }
    }
}
