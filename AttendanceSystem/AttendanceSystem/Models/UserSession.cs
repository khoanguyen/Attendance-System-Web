using AttendanceSystem.Infrastructure.Utils;
using AttendanceSystem.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Models
{
    public class UserSession
    {
        public const string LoggedinUserSession = "LoggedinUserSession";

        public LoginRequestType LoginType { get; private set; }

        public string TokenString { get; private set; }

        public List<MenuLinkModel> MainMenu { get; private set; }

        public UserSession()
        {
            MainMenu = new List<MenuLinkModel>();
        }
        public void SetUser(string token, bool isAdmin)
        {
            LoginType = isAdmin ? LoginRequestType.AdminLogin : LoginRequestType.StudentLogin;
            ConstructUserMenu(LoginType);
            TokenString = token;
            SessionHelper.SetSession(LoggedinUserSession, this);
        }

        public void SetMenuActive(int id)
        {
            var current = MainMenu.SingleOrDefault(m => m.IsActive == true);
            if (current.Id == id)
                return;

            current.IsActive = false;
            MainMenu.SingleOrDefault(m => m.Id == id).IsActive = true;
        }

        private void ConstructUserMenu(LoginRequestType type)
        {
            switch (type)
            {
                case LoginRequestType.StudentLogin:
                    MainMenu.Add(new MenuLinkModel() { Id = 1, Link = new LinkModel("Available Courses", "/aasstudent"), IsActive = true });
                    MainMenu.Add(new MenuLinkModel() { Id = 2, Link = new LinkModel("Registerd Courses", "/aasstudent/registeredcourses") });
                    break;
                case LoginRequestType.AdminLogin:
                    MainMenu.Add(new MenuLinkModel() { Id = 1, Link = new LinkModel("Courses", "/aasadmin/courses"), IsActive = true });
                    MainMenu.Add(new MenuLinkModel() { Id = 2, Link = new LinkModel("Students", "/aasadmin/students") });
                    MainMenu.Add(new MenuLinkModel() { Id = 3, Link = new LinkModel("Admins", "/aasadmin/admins") });
                    MainMenu.Add(new MenuLinkModel() {
                        Id = 4,
                        Link = new LinkModel("Add new", ""), 
                        ChildLinks = new List<LinkModel>() {
                            new LinkModel("New Student Account", "/aasadmin/student"),
                            //new LinkModel("New Admin Account", "/aasadmin/addadmin"),
                            new LinkModel("New Course", "/aasadmin/course")
                        }
                    });
                    break;
                default:
                    break;
            }           
        }
    }
}