using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Infrastructure.Errors
{
    public class TokenExchangeFailedException : BaseAASException
    {
        public TokenExchangeFailedException() : base("Token Exchange Failed") { }

        public override ErrorCode ErrorCode
        {
            get { return ErrorCode.TokenExchangeFailed; }
        }
    }
}