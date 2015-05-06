using AttendanceSystem.Infrastructure.Utils;
using AttendanceSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace AttendanceSystem.Infrastructure.Filters
{
    public class UserSessionFilterAttribute : ActionFilterAttribute
    {
        public LoginRequestType UserType;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (SessionHelper.IsSessionNull(UserSession.LoggedinUserSession))
            {
                filterContext.Result = new RedirectResult(HttpContext.Current.Request.Url.Scheme + Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host +
                    (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port.ToString()) + "/aaslogin");
            }else if (this.UserType != LoginRequestType.None && this.UserType != SessionHelper.GetSession<UserSession>(UserSession.LoggedinUserSession).LoginType)
            {
                filterContext.Result = filterContext.Result = new RedirectResult(HttpContext.Current.Request.Url.Scheme + Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host +
                    (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port.ToString()) + "/aaserror/authorization");
            }
        }

    }
}