using AttendanceSystem.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AttendanceSystem.Controllers
{
    [RoutePrefix("api/business")]
    public class LogicApiController : AASApiController
    {

        [Route("classes")]
        public IHttpActionResult GetClasses()
        {
            return Json(
                Logic.GetClasses()
                     .Select(c => new ClassLogicModel(c, false))
                );
        }

    }
}