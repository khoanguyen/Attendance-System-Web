using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AttendanceSystem.Controllers
{
    [RoutePrefix("api/public")]
    public class PublicTestApiController : ApiController
    {
        [HttpGet, Route("ping")]
        public IHttpActionResult Ping()
        {
            return Json("Echo");
        }
    }
}