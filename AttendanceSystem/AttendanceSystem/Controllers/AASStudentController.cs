using AttendanceSystem.Infrastructure.Filters;
using AttendanceSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttendanceSystem.Controllers
{
    [UserSessionFilter(UserType=LoginRequestType.StudentLogin)]
    public class AASStudentController : Controller
    {
        // GET: AASStudent
        public ActionResult Index()
        {
            return View();
        }
    }
}