using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Infrastructure.Utils
{
    public static class Config
    {
        public static int TokenExpiration { get; private set; }

        public static int TokenExchangeLimit { get; private set; }

        static Config()
        {
            TokenExpiration = int.Parse(ConfigurationManager.AppSettings["app:TokenExpiration"]);
            TokenExchangeLimit = int.Parse(ConfigurationManager.AppSettings["app:TokenExchangeLimit"]);
        }
    }
}