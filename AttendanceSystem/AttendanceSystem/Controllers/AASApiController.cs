using AttendanceSystem.Infrastructure;
using AttendanceSystem.Infrastructure.Errors;
using AttendanceSystem.Infrastructure.Implementation;
using AttendanceSystem.Infrastructure.Interfaces;
using AttendanceSystem.Infrastructure.Security;
using AttendanceSystem.Models;
using AttendanceSystem.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;

namespace AttendanceSystem.Controllers
{  
    public abstract class AASApiController : ApiController
    {
        private Student _currentStudent;

        public Student CurrentStudent
        {
            get
            {
                return _currentStudent ?? (_currentStudent = GetCurrentStudent());
            }
        }


        public IAASLogic Logic { get; private set; }

        public AASApiController()
        {
            Logic = AASLogic.Instance;
        }
        
        protected internal JsonResult<T> JsonEx<T>(T content)
        {
            return Json<T>(content, GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings);
        }

        private Student GetCurrentStudent()
        {
            var token = Token.ReadFromHeader(this.Request);
            var result = token != null && token.UserType == UserType.StudentUserType ?
                Logic.GetStudent(token.Username) :
                null;

            if (result == null)
                throw new InvalidTokenException();
            return result;
        }
    }
}