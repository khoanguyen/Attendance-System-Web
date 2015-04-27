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
                                     .ToArray();
                return classes;
            }
        }


        public Class GetClass(int id)
        {
            using (var context = new AASDBContext())
            {
                var classObj = context.Classes
                                      .Include("ClassSessions")
                                      .SingleOrDefault(c => c.Id == id);
                if (classObj == null) throw new NotFoundException();
                return classObj;
            }
        }


        public int AddClass(Class classObj)
        {
            using (var context = new AASDBContext())
            {
                context.Classes.Add(classObj);
                context.SaveChanges();
                return classObj.Id;
            }
        }

        public void UpdateClass(Class classObj)
        {
            using (var context = new AASDBContext())
            {
                if (!context.Classes.Any(c => c.Id == classObj.Id))
                    throw new NotFoundException();

                var existing = context.Classes.Include("ClassSessions").Single(c => c.Id == classObj.Id);

                existing.Name = classObj.Name;
                existing.ProfessorName = classObj.ProfessorName;
                existing.StartDate = classObj.StartDate;
                existing.EndDate = classObj.EndDate;
                existing.ExcusedTime = classObj.ExcusedTime;

                var newSessionList = classObj.ClassSessions.ToList();

                foreach (var session in existing.ClassSessions.ToArray())
                {
                    var newSession = newSessionList.SingleOrDefault(s => s.Id == session.Id);

                    if (newSession != null)
                    {
                        session.EndTime = newSession.EndTime;
                        session.StartTime = newSession.StartTime;
                        session.Weekday = newSession.Weekday;
                        session.Room = newSession.Room;
                        newSessionList.Remove(newSession);
                    }
                    else
                    {
                        context.ClassSessions.Remove(session);
                    }
                }

                foreach (var newSession in newSessionList)
                {
                    existing.ClassSessions.Add(newSession);                    
                }

                context.SaveChanges();                
            }
        }

        public void DeleteClass(int id)
        {
            using (var context = new AASDBContext())
            {
                var classObj = context.Classes.FirstOrDefault(c => c.Id == id);
                if (classObj != null)
                {
                    context.Classes.Remove(classObj);
                    context.SaveChanges();
                }
            }
        }

    }
}