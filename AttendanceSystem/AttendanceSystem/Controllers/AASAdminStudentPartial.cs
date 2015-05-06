using AttendanceSystem.Infrastructure.Filters;
using AttendanceSystem.Infrastructure.Utils;
using AttendanceSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttendanceSystem.Controllers
{
    public partial class AASAdminController : Controller
    {
        public ActionResult Students()
        {
            using (var client = SetupClientAndSetMenu(2))
            {

            }
            return View();
        }
    }
}