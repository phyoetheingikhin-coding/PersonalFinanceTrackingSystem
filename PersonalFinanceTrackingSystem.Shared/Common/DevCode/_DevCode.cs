using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace PersonalFinanceTrackingSystem.Shared.Common.DevCode
{
    public class _DevCode
    {

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
        //public string GenerateCode(string phone)
        //{
        //    var code;

        //}

        //public static string SHA256HexHashString(this string password, string phone)
        //{
        //    password = password.Trim();
        //    phone = phone.Trim();

        //    string saltedCode = EncodedbySalted(phone);//phone
        //    string hashString;
        //    using (var sha256 = SHA256.Create())
        //    {
        //        var hash = sha256.ComputeHash(Encoding.Default.GetBytes(password + saltedCode));
        //        hashString = ToHex(hash, false);
        //    }

        //    return hashString;
        //}
    }
}


