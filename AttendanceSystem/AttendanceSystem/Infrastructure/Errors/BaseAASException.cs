using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace AttendanceSystem.Infrastructure.Errors
{
    public abstract class BaseAASException : Exception
    {
        public abstract int ErrorCode { get; }

        public virtual HttpStatusCode HttpCode
        {
            get
            {
                return HttpStatusCode.BadRequest;
            }
        }

        public BaseAASException() : base() { }
        public BaseAASException(string message) : base(message) { }
    }
}