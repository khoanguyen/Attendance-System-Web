﻿using AttendanceSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttendanceSystem.Controllers
{
    public class AASErrorController : Controller
    {
        // GET: AASAuthorization

        public ActionResult Index(ErrorMessageModel error)
        {
            return View(error);
        }

        public ViewResult Authorization()
        {
            return View();
        }
    }
}