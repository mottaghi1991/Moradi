using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extention
{
    public static class UserExtention
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            try
            {
                var identifier = principal.Claims.SingleOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                if (identifier != null)
                {
                    return int.Parse(identifier.Value);
                }

                else
                {
                    return 0;
                }

                return identifier.Value == null ? 0 : int.Parse(identifier.Value);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}
