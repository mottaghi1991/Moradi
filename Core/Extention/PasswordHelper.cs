using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extention
{
    public static class PasswordHelper
    {
        public static string EncodePasswordMD5(string password)
        {
            byte[] originalBytes;
            byte[] encodedBytes;
            MD5 md5;
            md5 = new MD5CryptoServiceProvider();
            originalBytes = Encoding.Default.GetBytes(password);
            encodedBytes = md5.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string   
            return BitConverter.ToString(encodedBytes);
        }
    }
}
