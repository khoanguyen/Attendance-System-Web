using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AttendanceSystem.Infrastructure;

namespace AttendanceSystem.Models.LogicModel
{
    public class StudentLogicModel
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public bool ValidatePassword()
        {
            if (Password != ConfirmPassword) return false;

            if (Password.Length < 6) return false;

            return true;
        }

        public Student ToEntity()
        {
            var student = new Student();

            student.Password = this.Password;
            student.Id = this.Id;
            student.DisplayName = this.DisplayName;
            student.Email = this.Email;
            student.Salt = "";
            return student;
        }
    }
}