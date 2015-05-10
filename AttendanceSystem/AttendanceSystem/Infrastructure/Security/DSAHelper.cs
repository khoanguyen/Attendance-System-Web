using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace AttendanceSystem.Infrastructure.Security
{
    public static class DSAHelper
    {

        public static DSA GetPrivateDsa()
        {
            return GetDsa("key.private");
        }

        public static DSA GetPublicDsa()
        {
            return GetDsa("key.public");
        }

        public static DSA GetDsa(string filename)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", filename);
            var key = string.Empty;
            using (var reader = File.OpenText(path))
            {
                key = reader.ReadToEnd();
            }
            var dsa = DSA.Create();
            dsa.FromXmlString(key);
            return dsa;
        }
    }
}