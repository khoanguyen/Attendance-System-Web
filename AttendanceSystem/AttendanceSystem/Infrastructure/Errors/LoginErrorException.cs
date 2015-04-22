using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace AttendanceSystem.Infrastructure.Errors
{
    public class LoginErrorException : BaseAASException
    {
        public override int ErrorCode
        {
            get { return 1; }
        }

        public override System.Net.HttpStatusCode HttpCode
        {
            get
            {
                return HttpStatusCode.Unauthorized;
            }
        }

        public LoginErrorException()
            : base("Login failed")
        { }

    }
}