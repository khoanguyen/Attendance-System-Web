using AttendanceSystem.Infrastructure.Errors;
using AttendanceSystem.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace AttendanceSystem.Infrastructure.Security
{
    [Serializable]
    public sealed class Token
    {

        public static string SecurityHeaderName = "aas-accesskey";

        public string Username { get; private set; }

        public string UserType { get; private set; }

        public DateTimeOffset Expiration { get; private set; }

        public string Signature { get; private set; }

        private Token(string username, string userType, DateTimeOffset expiration)
        {
            Username = username;
            UserType = userType;
            Expiration = expiration;
        }

        public static Token CreateAndSign(string username, string userType, DateTimeOffset expiration)
        {
            var expriration = DateTimeOffset.Now + TimeSpan.FromDays(15);
            var token = new Token(username, userType, expiration);
            SignToken(token);
            return token;
        }

        public static Token ReadFromString(string tokenString)
        {
            var bytes = ConvertToBytes(tokenString.Trim());
            var token = DeserializeToken(bytes);
            return token;
        }

        public static string ReadTokenStringFromHeader(HttpRequestMessage request)
        {
            if (request.Headers.Any(h => h.Key.ToLower() == SecurityHeaderName.ToLower()))
            {
                var header = request.Headers.First(h => h.Key.ToLower() == SecurityHeaderName.ToLower());
                var content = header.Value.FirstOrDefault() ?? string.Empty;
                return content.Trim();
            }
            return null;
        }

        public static Token ReadFromHeader(HttpRequestMessage request)
        {
            var tokenString = ReadTokenStringFromHeader(request);
            if (!string.IsNullOrWhiteSpace(tokenString)) return ReadFromString(tokenString);
            return null;
        }

        public Token Exchange()
        {
            var newExpired = this.Expiration + TimeSpan.FromSeconds(Config.TokenExpiration);
            return CreateAndSign(this.Username, this.UserType, newExpired);
        }

        public override string ToString()
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                var bytes = ms.ToArray();
                return ConvertToString(bytes);
            }
        }

        public bool IsExpired()
        {
            return this.Expiration < DateTimeOffset.Now;
        }

        public bool IsExchangable()
        {
            return DateTimeOffset.Now < (this.Expiration + TimeSpan.FromSeconds(Config.TokenExchangeLimit));
        }

        private static Token DeserializeToken(byte[] bytes)
        {
            try
            {
                using (var ms = new MemoryStream(bytes))
                {
                    var formatter = new BinaryFormatter();
                    var token = (Token)formatter.Deserialize(ms);                    
                    return token;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidTokenException();
            }
        }

        private static byte[] ConvertToBytes(string data)
        {
            string refinedData = data.Replace('_', '/').Replace('-', '+');

            switch (refinedData.Length % 4)
            {
                case 2: refinedData += "=="; break;
                case 3: refinedData += "="; break;
            }

            var bytes = Convert.FromBase64String(refinedData);
            return bytes;           
        }

        private static string ConvertToString(byte[] bytes)
        {
            string result = Convert.ToBase64String(bytes)
                                   .TrimEnd('=')
                                   .Replace('+', '-')
                                   .Replace('/', '_');
            return result;
        }

        private static void SignToken(Token token)
        {
            using (var dsa = DSAHelper.GetPrivateDsa())
            {
                var rgbHash = token.ComposeData();
                var sigBytes = dsa.CreateSignature(rgbHash);
                var sigStr = ConvertToString(sigBytes);
                token.Signature = sigStr;
            }
        }


        public void Validate()
        {
            using (var dsa = DSAHelper.GetPublicDsa())
            {
                var sigBytes = ConvertToBytes(this.Signature);
                var data = this.ComposeData();
                
                if (IsExpired()) throw new TokenExpiredException();

                if (!dsa.VerifySignature(data, sigBytes) && DateTimeOffset.Now < this.Expiration)
                    throw new InvalidTokenException();
            }
        }

        private byte[] ComposeData()
        {
            using (var sha1 = SHA1.Create())
            {
                var data = string.Format("{0}/{1}/{2}", Username, UserType, Expiration);
                var rgbHash = sha1.ComputeHash(Encoding.ASCII.GetBytes(data));
                return rgbHash;
            }
        } 
    }
}