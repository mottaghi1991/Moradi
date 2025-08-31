using Microsoft.AspNetCore.Mvc;

namespace WebStore.Base
{
    public class BaseController : Microsoft.AspNetCore.Mvc.Controller
    {

        public static string Success = "Success";
        public static string Error = "Error";
        public static string SuccessMessage = "عملیات با موفقیت انجام شد .";
        public static string ErrorMessage = "عملیات با خطا مواجه شد .";
        public static string warning = "warning";

        public static class AreaName
        {
            public const string UserPanel = "UserPanel";
            public const string Admin = "Admin";
        }
    }

}
