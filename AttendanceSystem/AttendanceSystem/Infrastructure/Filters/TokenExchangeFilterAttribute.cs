using AttendanceSystem.Infrastructure.Errors;
using AttendanceSystem.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace AttendanceSystem.Infrastructure.Filters
{
    public class TokenExchangeFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var token = Token.ReadFromHeader(actionContext.Request);
            if (token == null) throw new InvalidTokenException();
            if (!token.IsExchangable()) throw new TokenExchangeFailedException();
        }
    }
}