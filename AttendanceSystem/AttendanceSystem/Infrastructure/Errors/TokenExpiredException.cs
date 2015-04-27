using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Infrastructure.Errors
{
    public class TokenExpiredException : BaseAASException
    {
        public override ErrorCode ErrorCode
        {
            get { return ErrorCode.TokenExpired; }
        }

        public override System.Net.HttpStatusCode HttpCode
        {
            get
            {
                return System.Net.HttpStatusCode.Forbidden;
            }
        }

        public TokenExpiredException() : base("Security Token is expired") { }
    }
}