using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace AttendanceSystem.Infrastructure.Errors
{
    public class LoginErrorException : BaseAASException
    {
        public override ErrorCode ErrorCode
        {
            get { return ErrorCode.LoginError; }
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