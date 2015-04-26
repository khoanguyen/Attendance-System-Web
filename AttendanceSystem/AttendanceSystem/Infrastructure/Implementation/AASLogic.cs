using AttendanceSystem.Infrastructure.Errors;
using AttendanceSystem.Infrastructure.Interfaces;
using AttendanceSystem.Infrastructure.Security;
using AttendanceSystem.Infrastructure.Utils;
using AttendanceSystem.Models;
using AttendanceSystem.Models.LogicModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Infrastructure.Implementation
{
    public class AASLogic : IAASLogic
    {
        private const string InvalidRequestTypeMessage = "Invalid Login Request Type";
        private const string InvalidUsernameOrPassword = "Invalid Username or Password";

        public static AASLogic Instance { get; private set; }

        static AASLogic()
        {
            Instance = new AASLogic();
        }

        public string SignIn(Models.LogicModel.LoginLogicModel model)
        {
            switch (model.RequestType)
            {
                case LoginRequestType.AdminLogin: return SignInAdmin(model);
                default: return InvalidRequestTypeMessage;
            }
        }

        private string SignInAdmin(LoginLogicModel model)
        {
            using (var context = new AASDBContext())
            {
                var admin = context.Admins.SingleOrDefault(a => a.Username == model.Username);
                if (admin == null) 
                    throw new LoginErrorException();

                if (string.IsNullOrWhiteSpace(admin.Salt))
                {                    
                    admin.Salt = PasswordHashProvider.GenerateSalt();
                    admin.Password = PasswordHashProvider.ComputePasswordHash(admin.Password.Trim(), admin.Salt);
                    context.SaveChanges();
                }

                var hash = PasswordHashProvider.ComputePasswordHash(model.Password, admin.Salt);

                if (admin.Password != hash)
                    throw new LoginErrorException();

                return ComposeToken(model.Username, "Admin");
            }
        }

        private string ComposeToken(string username, string userType)
        {
            var expiration = DateTimeOffset.Now + TimeSpan.FromSeconds(Config.TokenExpiration);
            var token = Token.CreateAndSign(username, userType, expiration);
            return token.ToString();
        }

        public string ExchangeToken(string tokenString)
        {
            var token = Token.ReadFromString(tokenString);

            if (!token.IsExchangable())
                throw new TokenExpiredException();

            return token.Exchange().ToString();
        }


        public IEnumerable<Class> GetClasses()
        {
            using (var context = new AASDBContext())
            {
                var classes = context.Classes
                                     .Include("ClassSessions")
                                     .ToArray();
                return classes;
            }
        }
    }
}