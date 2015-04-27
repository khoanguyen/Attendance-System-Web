using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using WebGrease.Configuration;

namespace AttendanceSystem.Infrastructure.Errors
{
    public class ApiExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            if (context.Exception is BaseAASException)
            {
                var apiException = context.Exception as BaseAASException;
                context.Result = new ApiErrorActionResult(context.Request, apiException.HttpCode, apiException);                
            }
            else
            {
                base.Handle(context);
            }
        }
    }
}