using AttendanceSystem.Models;
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
        Class GetClass(int id);
        int AddClass(Class classObj);
        void UpdateClass(Class classObj);
        void DeleteClass(int id);
    }
}