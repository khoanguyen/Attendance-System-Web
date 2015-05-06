using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Infrastructure.Utils
{
    public class SessionHelper
    {

        public static void SetSession<T>(string sessionName, T value)
        {
            HttpContext.Current.Session[sessionName] = value;
        }

        public static TOutput GetSession<TOutput>(string sessionName)
        {
            return (TOutput)HttpContext.Current.Session[sessionName];
        }

        public static bool IsSessionNull(string sessionName)
        {
            return HttpContext.Current.Session[sessionName] == null;
        }

        public static void ClearSession(string sessionName)
        {
            HttpContext.Current.Session.Remove(sessionName);
        }

        public static void ClearAllSession()
        {
            HttpContext.Current.Session.RemoveAll();
        }
    }
}