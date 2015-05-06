using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Infrastructure.Utils
{
    public class ApiUriHelper
    {
        public const string allClassesUrl = "business/classes";
        public const string classByIdUrl = "business/classes/{0}";
        
        public static string GetBaseUri()
        {
            return ConfigurationManager.AppSettings["webapp:BaseAPIUrl"];
        }

        public static string ComposeUrl(string urlPattern, params string[] args)
        {
            return String.Format(urlPattern, args);
        }
    }
}