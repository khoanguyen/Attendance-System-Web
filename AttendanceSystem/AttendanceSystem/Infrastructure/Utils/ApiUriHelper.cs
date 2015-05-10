using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Infrastructure.Utils
{
    public class ApiUriHelper
    {
        public const string AllClassesUrl = "business/classes";
        public const string ClassByIdUrl = "business/classes/{0}";
        public const string StudentUrl = "business/students";
        public const string StudentById = "business/student/{0}";
        public const string GetAvailableClassesUrl = "business/classes/available";
        public const string GetRegisteredClassesUrl = "business/classes/registered";
        public const string RegisterClassUrl = "business/classes/register/{0}";
        public const string DropClassUrl = "business/classes/drop/{0}"; 
        
        public static string GetBaseUri()
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseAPIUrl"];
            return baseUrl;
        }

        public static string ComposeUrl(string urlPattern, params string[] args)
        {
            return String.Format(urlPattern, args);
        }
    }
}