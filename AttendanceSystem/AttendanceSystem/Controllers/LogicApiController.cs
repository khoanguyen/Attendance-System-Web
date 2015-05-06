using AttendanceSystem.Infrastructure.Filters;
using AttendanceSystem.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AttendanceSystem.Controllers
{
    [RoutePrefix("api/business"), TokenRequiredFilter]
    public class LogicApiController : AASApiController
    {

        [HttpGet, Route("classes")]
        public IHttpActionResult GetClasses()
        {
            return JsonEx(
                Logic.GetClasses()
                     .Select(c => new ClassLogicModel(c, false))
                );
        }

        [HttpGet, Route("classes/{id:int}")]
        public IHttpActionResult GetClass([FromUri]int id)
        {
            return JsonEx(new ClassLogicModel(Logic.GetClass(id)));
        }

        [HttpPost, Route("classes")]
        public IHttpActionResult AddClass([FromBody] ClassLogicModel classModel)
        {
            var entity = classModel.ToEntity();
            Logic.AddClass(entity);
            return JsonEx(entity.Id);
        }

        [HttpPut, Route("classes/{id}")]
        public IHttpActionResult UpdateClass([FromUri] int id, [FromBody] ClassLogicModel classModel)
        {
            classModel.Id = id;
            var entity = classModel.ToEntity();
            Logic.UpdateClass(entity);
            return Ok();
        }

        [HttpDelete, Route("classes/{id}")]
        public IHttpActionResult DeleteClass([FromUri] int id)
        {
            Logic.DeleteClass(id);
            return Ok();
        }

        [HttpGet, Route("classes/available")]
        public IHttpActionResult GetAvailableClassForUser()
        {
            return JsonEx(Logic.GetAvailableClassForStudent(CurrentStudent.Email));
        }

        [HttpGet, Route("classes/registered")]
        public IHttpActionResult GetRegisteredClassForUser()
        {
            return JsonEx(Logic.GetRegisteredClassForStudent(CurrentStudent.Email));
        }
    }
}