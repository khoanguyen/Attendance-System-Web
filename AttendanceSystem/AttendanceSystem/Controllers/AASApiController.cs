using AttendanceSystem.Infrastructure.Errors;
using AttendanceSystem.Infrastructure.Implementation;
using AttendanceSystem.Infrastructure.Interfaces;
using AttendanceSystem.Infrastructure.Security;
using AttendanceSystem.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace AttendanceSystem.Controllers
{  
    public abstract class AASApiController : ApiController
    {
        public IAASLogic Logic { get; private set; }

        public AASApiController()
        {
            Logic = AASLogic.Instance;
        }

    }
}