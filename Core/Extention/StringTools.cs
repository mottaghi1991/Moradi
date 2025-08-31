using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guid = System.Guid;

namespace Core.Extention
{
    public static class StringTools
    {
        public static string FixEmail(string email)
        {
            return email.Trim().ToLower();
        }

        public static string GenerateUniqeCode()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
