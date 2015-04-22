using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace AttendanceSystem.Infrastructure.Utils
{
    public static class PasswordHashProvider
    {
        public static string GenerateSalt()
        {
            var data = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString());
            return HashData(data);
        }

        public static string ComputePasswordHash(string password, string salt)
        {
            var dataStr = string.Format("{0}+{1}", salt, password);
            var data = Encoding.ASCII.GetBytes(dataStr);
            return HashData(data);

        }

        private static string HashData(byte[] data)
        {
            using (var sha = SHA256.Create())
            {
                var cipher = sha.ComputeHash(data);
                return string.Concat(cipher.Select(b =>
                {
                    var result = string.Format("{0,2:X2}", b);
                    return result;
                }));
            }
        }
    }
}