using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Infrastructure.Errors
{
    public enum ErrorCode
    {
        NotFound,
        InvalidToken,
        LoginError,
        TokenExchangeFailed,
        TokenExpired
    }
}