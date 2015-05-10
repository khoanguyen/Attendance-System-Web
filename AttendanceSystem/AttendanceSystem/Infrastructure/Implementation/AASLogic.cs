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


        #region Private helpers

        private string ComposeToken(string username, string userType)
        {
            var expiration = DateTimeOffset.Now + TimeSpan.FromSeconds(Config.TokenExpiration);
            var token = Token.CreateAndSign(username, userType, expiration);
            return token.ToString();
        }

        private string SignInStudent(LoginLogicModel model)
        {
            using (var context = new AASDBContext())
            {
                var student = context.Students.SingleOrDefault(a => a.Email == model.Username);
                if (student == null)
                    throw new LoginErrorException();

                if (string.IsNullOrWhiteSpace(student.Salt))
                {
                    student.Salt = PasswordHashProvider.GenerateSalt();
                    student.Password = PasswordHashProvider.ComputePasswordHash(student.Password.Trim(), student.Salt);
                    context.SaveChanges();
                }

                var hash = PasswordHashProvider.ComputePasswordHash(model.Password, student.Salt);

                if (student.Password != hash)
                    throw new LoginErrorException();

                return ComposeToken(model.Username, UserType.StudentUserType);
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

                return ComposeToken(model.Username, UserType.AdminUserType);
            }
        }
        #endregion


        public string SignIn(Models.LogicModel.LoginLogicModel model)
        {
            switch (model.RequestType)
            {
                case LoginRequestType.AdminLogin: return SignInAdmin(model);
                case LoginRequestType.StudentLogin: return SignInStudent(model);
                default: return InvalidRequestTypeMessage;
            }
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
                                     .Where(c => c.IsArchived == false)
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
                    //This code cause "Multiplicity constraint violated" exception
                    //existing.ClassSessions.Add(newSession);
                    existing.ClassSessions.Add(new ClassSession
                    {
                        Class = existing,
                        ClassId = existing.Id,
                        EndTime = newSession.EndTime,
                        StartTime = newSession.StartTime,
                        Room = newSession.Room,
                        Weekday = newSession.Weekday
                    });                    
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
                else
                {
                    throw new NotFoundException();
                }
            }
        }


        public Student GetStudent(string email)
        {
            using (var context = new AASDBContext())
            {
                var student = context.Students.SingleOrDefault(s => s.Email == email);

                if (student == null)
                    throw new NotFoundException();

                return student;
            }
        }


        public IEnumerable<Class> GetAvailableClassForStudent(string userName)
        {
            using (var context = new AASDBContext())
            {
                var now = DateTime.Now.Date;
                var student = GetStudent(userName);
                var studentId = student.Id;
                return context.Classes
                              .Include("ClassSessions")
                              .Where(c => c.StartDate <= now &&
                                                         now <= c.EndDate &&
                                                         c.IsArchived == false &&
                                                         !c.Tickets.Any(t => t.StudentId == studentId && t.ClassId == c.Id))
                              .ToArray()
                              .Select(c =>
                              {
                                  c.Tickets = null;
                                  return c;
                              });
            }
        }

        public IEnumerable<Class> GetRegisteredClassForStudent(string userName)
        {
            using (var context = new AASDBContext())
            {
                var now = DateTime.Now.Date;
                var student = GetStudent(userName);
                var studentId = student.Id;
                return context.Classes
                              .Include("ClassSessions")                              
                              .Where(c => c.StartDate <= now &&
                                                         now <= c.EndDate &&
                                                         c.IsArchived == false &&
                                                         c.Tickets.Any(t => t.StudentId == studentId && t.ClassId == c.Id))
                              .ToArray()
                              .Select(c =>
                              {
                                  c.Tickets = null;
                                  return c;
                              });
            }
        }


        public IEnumerable<Admin> GetAdmins()
        {
            using (var context = new AASDBContext())
            {
                return context.Admins.Where(a => a.Status < AdminStatus.TrumCuoi);
            }
        }

        public Admin GetAdmin(string username)
        {
            using (var context = new AASDBContext())
            {
                var admin = context.Admins.Where(a => a.Status < AdminStatus.TrumCuoi)
                                          .SingleOrDefault(a => a.Username == username);

                if (admin == null)
                    throw new NotFoundException();

                return admin;
            }   
        }

        public void AddAdmin(Admin admin)
        {
            using (var context = new AASDBContext())
            {
                admin.Status = AdminStatus.Active;
                var userName = admin.Username;
                var existing = context.Admins.Where(a => a.Status < AdminStatus.TrumCuoi)
                                             .SingleOrDefault(a => a.Username == userName);
                if (existing != null)
                {
                    existing.AdminName = admin.AdminName;
                    existing.Status = admin.Status;
                    existing.Salt = admin.Salt;
                    existing.Password = admin.Password;
                }
                else
                {
                    context.Admins.Add(admin);
                }

                context.SaveChanges();                
            }  
        }

        public void DeleteAdmin(string username)
        {
            using (var context = new AASDBContext())
            {
                var admin = GetAdmin(username);
                admin.Status = AdminStatus.Deleted;
                context.SaveChanges();
            }
        }

        public void UpdateAdmin(Admin admin)
        {
            using (var context = new AASDBContext())
            {
                var existing = GetAdmin(admin.Username);

                if (existing == null)
                    throw new NotFoundException();

                existing.AdminName = admin.AdminName;

                context.SaveChanges();
            }
        }

        public Ticket RegisterClass(int studentId, int classId)
        {
            using (var context = new AASDBContext())
            {
                var ticket = context.Tickets.SingleOrDefault(t => t.StudentId == studentId && t.ClassId == classId);
                if (ticket != null) return ticket;

                ticket = new Ticket
                {
                    StudentId = studentId,
                    ClassId = classId
                };

                ticket.GenerateQrCode();

                context.Tickets.Add(ticket);
                context.SaveChanges();

                return ticket;
            }
        }

        public void DropClass(int studentId, int classId)
        {
            using (var context = new AASDBContext())
            {
                var ticket = context.Tickets.SingleOrDefault(t => t.StudentId == studentId && t.ClassId == classId);
                if (ticket != null)
                { 
                    context.Tickets.Remove(ticket);
                    context.SaveChanges();
                }               
            }
        }

        public byte[] GetTicketQrCode(int studentId, int classId)
        {
            using (var context = new AASDBContext())
            {
                var ticket = context.Tickets.SingleOrDefault(t => t.StudentId == studentId && t.ClassId == classId);
                if (ticket != null)
                {
                    return ticket.QrCode;
                }

                return new byte[0];
            }
        }

        public bool CheckQrCode(int studentId, int classId, string qrCode)
        {
            using (var context = new AASDBContext())
            {
                var ticket = context.Tickets.SingleOrDefault(t => t.StudentId == studentId && t.ClassId == classId);
                if (ticket != null)
                {
                    return ticket.Verify(qrCode);
                }

                return false;
            }
        }

        public Class GetClassWithTicket(int id)
        {
            using (var context = new AASDBContext())
            {
                var classObj = context.Classes
                                      .Include("Tickets")
                                      .Include("ClassSessions")
                                      .SingleOrDefault(c => c.Id == id);

                if (classObj == null) throw new NotFoundException();
                return classObj;
            }
        }

        public IEnumerable<Class> GetClassesWithTicket(int studentId)
        {
            using (var context = new AASDBContext())
            {
                var classes = context.Classes
                                     .Where(c => c.IsArchived == false)                                     
                                     .ToArray()
                                     .Select(c => new Class
                                     {
                                         EndDate = c.EndDate,
                                         ExcusedTime = c.ExcusedTime,
                                         Id = c.Id,
                                         IsArchived = c.IsArchived,
                                         Name = c.Name,
                                         ProfessorName = c.ProfessorName,
                                         StartDate = c.StartDate
                                     }).ToArray();
                var tickets = context.Tickets.Where(t => t.StudentId == studentId).ToArray();
                foreach (var ticket in tickets)
                {
                    var classObj = classes.SingleOrDefault(c => c.Id == ticket.ClassId);
                    if (classObj != null)
                    {
                        classObj.Tickets.Add(ticket);
                    }
                }
                return classes;
            }
        }
    } 
}