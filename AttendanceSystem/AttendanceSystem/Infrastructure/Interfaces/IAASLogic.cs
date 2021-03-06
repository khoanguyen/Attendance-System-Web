﻿using AttendanceSystem.Models;
using AttendanceSystem.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Infrastructure.Interfaces
{
    public interface IAASLogic
    {
        string ExchangeToken(string tokenString);
        string SignIn(LoginLogicModel model);

        IEnumerable<Class> GetClasses();
        IEnumerable<Class> GetClassesWithTicket(int studentId);
        Class GetClass(int id);
        Class GetClassWithTicket(int id);
        int AddClass(Class classObj);
        void UpdateClass(Class classObj);
        void DeleteClass(int id);
        IEnumerable<Class> GetAvailableClassForStudent(string userName);
        IEnumerable<Class> GetRegisteredClassForStudent(string userName);

        IEnumerable<Admin> GetAdmins();
        Admin GetAdmin(string username);
        void AddAdmin(Admin admin);
        void DeleteAdmin(string userName);
        void UpdateAdmin(Admin admin);

        Student GetStudent(string email);
        IEnumerable<Student> GetStudents();
        int AddStudent(Student student);
        Ticket RegisterClass(int studentId, int classId);
        void DropClass(int studentId, int classId);
        byte[] GetTicketQrCode(int studentId, int classId);
        bool CheckQrCode(int studentId, int classId, string qrCode);
    }
}