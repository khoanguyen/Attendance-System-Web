using AttendanceSystem.Infrastructure.Errors;
using AttendanceSystem.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace AttendanceSystem.Infrastructure.Filters
{
    public class TokenRequiredFilterAttribute : ActionFilterAttribute
    {
        public string RequiredUserType { get; set; }        

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var token = Token.ReadFromHeader(actionContext.Request);
            if (token == null) throw new InvalidTokenException();
            token.Validate();
            
            if (!string.IsNullOrEmpty(RequiredUserType))
            {
                if (token.UserType.ToLower().Trim() != RequiredUserType.ToLower().Trim())
                    throw new InvalidTokenException();
            }
        }
    }
}