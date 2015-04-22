using AttendanceSystem.Infrastructure;
using AttendanceSystem.Infrastructure.Filters;
using AttendanceSystem.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AttendanceSystem.Controllers
{
    [RoutePrefix("api/test"), TokenRequiredFilter]
    public class TestApiController: ApiController
    {
        [HttpGet, Route("salt")]
        public IHttpActionResult TestSalt()
        {
            var hash = PasswordHashProvider.GenerateSalt();
            return Json(new
            {
                Hash = hash,
                Length = hash.Length
            });
        }
    }
}