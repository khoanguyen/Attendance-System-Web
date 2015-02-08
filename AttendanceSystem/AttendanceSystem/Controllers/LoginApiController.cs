using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AttendanceSystem.Controllers
{
    [RoutePrefix("api/privacy")]
    public class LoginApiController : ApiController
    {

        [HttpPost, Route("login")]
        public IHttpActionResult Login()
        {
            return Json(new
            {
                Name = "StudentName",
                StudentId = "88344",
                Email = "email@gmail.com"
            });
        }
    }
}
