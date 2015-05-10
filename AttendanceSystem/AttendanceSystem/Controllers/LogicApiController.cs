using AttendanceSystem.Infrastructure.Filters;
using AttendanceSystem.Infrastructure.Utils;
using AttendanceSystem.Models;
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
            var classes = (CurrentStudent != null) ? Logic.GetClassesWithTicket(CurrentStudent.Id) : Logic.GetClasses();
            return JsonEx(classes.Select(c => new ClassLogicModel(c, false)));
        }

        [HttpGet, Route("classes/{id:int}")]
        public IHttpActionResult GetClass([FromUri]int id)
        {            
            var classObj = (CurrentStudent != null)? Logic.GetClassWithTicket(id): Logic.GetClass(id);
            var model = new ClassLogicModel(classObj);
            if (CurrentStudent != null)
            {
                int currentStudentId = CurrentStudent.Id;
                Ticket ticket = classObj.Tickets.FirstOrDefault(t => t.ClassId == id && t.StudentId == currentStudentId);
                model.OwnedTicket = ticket == null ? null : new TicketModel(ticket);
            }
            return JsonEx(model);
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

        [HttpPost, Route("classes/register/{classId}")]
        public IHttpActionResult RegisterClass([FromUri]int classId)
        {
            var ticket = Logic.RegisterClass(CurrentStudent.Id, classId);
            return JsonEx(ticket);
        }

        [HttpPost, Route("classes/drop/{classId}")]
        public IHttpActionResult DropClass([FromUri]int classId)
        {
            Logic.DropClass(CurrentStudent.Id, classId);
            return Ok();
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
        public IHttpActionResult CheckQrCode([FromUri]int classId, [FromBody] QrCodeModel model)
        {
            int studentId = Ticket.ExtractStudentId(model.QrCode);
            if (studentId != CurrentStudent.Id)
            {
                return JsonEx(new
                {
                    result = false
                });
            }

            return JsonEx(new
            {
                result = Logic.CheckQrCode(studentId, classId, model.QrCode)
            });
        }
    }
}