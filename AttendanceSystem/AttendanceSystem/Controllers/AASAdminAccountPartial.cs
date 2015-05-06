using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttendanceSystem.Models;

namespace AttendanceSystem.Controllers
{
    public partial class AASAdminController : Controller
    {
        public ViewResult Admins()
        {
            using (var client = SetupClientAndSetMenu(3))
            {

            }
            return View();
        }
    }
}