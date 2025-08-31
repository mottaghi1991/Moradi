using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Attribute
{
    public class RediretAuthenticateAttribute : ActionFilterAttribute
    {
        private readonly string _rediretUrl;
        public RediretAuthenticateAttribute(string rediretUrl="/")
        {
            _rediretUrl = rediretUrl;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            if(user?.Identity!=null &&user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectResult(_rediretUrl);
            }
            base.OnActionExecuting(context);
        }
    }
}
