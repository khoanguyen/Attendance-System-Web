using AttendanceSystem.Infrastructure.Filters;
using AttendanceSystem.Models;
using AttendanceSystem.Infrastructure.Utils;
using AttendanceSystem.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
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

        [HttpGet, Route("students")]
        public IHttpActionResult GetStudents()
        {
            return JsonEx(Logic.GetStudents());
        }

        [HttpPost, Route("students")]
        public IHttpActionResult AddStudent([FromBody] StudentLogicModel student)
        {
            var entity = student.ToEntity();
            Logic.AddStudent(entity);
            return JsonEx(entity.Id);
        }


        [HttpPost, Route("classes/register/{classId}")]
        public IHttpActionResult RegisterClass([FromUri]int classId)
        {
            var ticket = Logic.RegisterClass(CurrentStudent.Id, classId);
            return JsonEx(ticket);
        }

        [HttpPost, Route("classes/drop/{classId}")]
        public IHttpActionResult DropClass([FromUri]int classId)
        {
            var ticket = Logic.RegisterClass(CurrentStudent.Id, classId);
            return JsonEx(ticket);
        }

        [HttpGet, Route("qrCode/{classId}")]
        public IHttpActionResult GetQrCode([FromUri]int classId)
        {
            var image = Logic.GetTicketQrCode(CurrentStudent.Id, classId);
            return JsonEx(new
            {
                image = Convert.ToBase64String(image)
            });
        }

        [HttpPost, Route("qrCode/{classId}")]
        public IHttpActionResult CheckQrCode([FromUri]int classId, [FromUri]int studentId, [FromBody] QrCodeModel model)
        {            
            return JsonEx(new
            {
                result = Logic.CheckQrCode(CurrentStudent.Id, classId, model.QrCode)
            });
        }
    }
}