using AttendanceSystem.Infrastructure;
using AttendanceSystem.Infrastructure.Filters;
using AttendanceSystem.Infrastructure.Utils;
using AttendanceSystem.Models;
using AttendanceSystem.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net.Http;

namespace AttendanceSystem.Controllers
{
    public partial class AASAdminController : Controller
    {
        public async Task<ActionResult> Students()
        {
            using (var client = SetupClientAndSetMenu(2))
            {
                var url = ApiUriHelper.GetBaseUri() + ApiUriHelper.StudentUrl;
                try
                {
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var serializer = new JavaScriptSerializer();
                        var students = serializer.Deserialize<List<Student>>(data);
                        return View(students);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Error");
                    }
                }
                catch 
                {
                    return RedirectToAction("Index", "Error");
                }                        
            }            
        }

        public ActionResult Student()
        {
            return View(new StudentLogicModel());  
        }

        [HttpPost]
        public async Task<ActionResult> Student(StudentLogicModel student)
        {
            if (!student.ValidatePassword())
            {
                ViewBag.Error = "Password must be at least 6 characters. Password and confirm password must match.";
                return View(student);
            }
            using (var client = SetupClientAndSetMenu(2))
            {
                var url = ApiUriHelper.GetBaseUri() + ApiUriHelper.StudentUrl;
                try
                {
                    var response = await client.PostAsJsonAsync<StudentLogicModel>(url, student);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Students");
                    }
                    else
                    {
                        ViewBag.Error = "Can't create student account";
                        return View(student);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error: " + ex.Message;
                    return View(student);
                }                
            }
        }
    }
}