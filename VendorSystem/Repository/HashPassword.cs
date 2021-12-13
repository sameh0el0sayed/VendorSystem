using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace VendorSystem.Repository
{
    public static class HashPassword
    {
        public static string Hash(string password)
        {
            var bytes = new UTF8Encoding().GetBytes(password);
            var hashBytes = System.Security.Cryptography.MD5.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}