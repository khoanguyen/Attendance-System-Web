using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace AttendanceSystem.Infrastructure.Errors
{
    public class NotFoundException : BaseAASException
    {
        public NotFoundException() : base("Resource not found") { }

        public override ErrorCode ErrorCode
        {
            get { return ErrorCode.NotFound; }
        }

        public override System.Net.HttpStatusCode HttpCode
        {
            get
            {
                return HttpStatusCode.NotFound;
            }
        }
    }
}