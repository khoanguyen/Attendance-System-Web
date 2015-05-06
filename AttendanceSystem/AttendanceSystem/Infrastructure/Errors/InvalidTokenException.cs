using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace AttendanceSystem.Infrastructure.Errors
{
    public class InvalidTokenException : BaseAASException
    {

        public override ErrorCode ErrorCode
        {
            get { return ErrorCode.InvalidToken; }
        }

        public override System.Net.HttpStatusCode HttpCode
        {
            get
            {
                return HttpStatusCode.Forbidden;
            }
        }        

        public InvalidTokenException() : base("Invalid Security Token") { }

    }
}