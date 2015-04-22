using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace AttendanceSystem.Infrastructure.Errors
{
    public class InvalidTokenException : BaseAASException
    {

        public override int ErrorCode
        {
            get { return 2; }
        }

        public override System.Net.HttpStatusCode HttpCode
        {
            get
            {
                return HttpStatusCode.Forbidden;
            }
        }        

        public InvalidTokenException() : base("Invalid Secuirty Token") { }

    }
}