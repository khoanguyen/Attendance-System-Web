using AttendanceSystem.Infrastructure.Filters;
using AttendanceSystem.Infrastructure.Utils;
using AttendanceSystem.Models;
using AttendanceSystem.Models.LogicModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AttendanceSystem.Controllers
{
    [UserSessionFilter(UserType=LoginRequestType.StudentLogin)]
    public class AASStudentController : Controller
    {
        // GET: AASStudent
        public async Task<ActionResult> Index()
        {
            using (var client = SetupClientAndSetMenu(1))
            {
                var url = ApiUriHelper.GetBaseUri() + ApiUriHelper.GetAvailableClassesUrl;
                                    
                try
                {
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var serializer = new JavaScriptSerializer();
                        var courses = serializer.Deserialize<List<Class>>(data);
                        return View(courses);
                    }
                    else
                    {
                        return RedirectToAction("index", "error");
                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("index", "error");
                }
            }
        }

        public async Task<ActionResult> RegisteredCourses()
        {
            using (var client = SetupClientAndSetMenu(2))
            {
                var url = ApiUriHelper.GetBaseUri() + ApiUriHelper.GetRegisteredClassesUrl;

                try
                {
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var serializer = new JavaScriptSerializer();
                        var courses = serializer.Deserialize<List<Class>>(data);
                        return View(courses);
                    }
                    else
                    {
                        return RedirectToAction("index", "aaserror");
                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("index", "aaserror");
                }
            }
        }

        public async Task<ActionResult> ViewCourse(int id)
        {
            using (var client = SetupClientAndSetMenu(null))
            {
                var url = ApiUriHelper.GetBaseUri() + ApiUriHelper.ComposeUrl(ApiUriHelper.ClassByIdUrl, id.ToString());
                try
                {
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var course = JsonConvert.DeserializeObject<ClassLogicModel>(data);
                        if (course.OwnedTicket != null && course.OwnedTicket.QrCode != null)
                        {
                            course.OwnedTicket.QrCode = @"data:image/png;base64," + course.OwnedTicket.QrCode;
                        }
                        return View(course);
                    }
                    return RedirectToAction("index", "aaserror");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("index", "aaserror");
                }
            }
        }

        public async Task<ActionResult> RegisterCourse(int id, string courseName = "")
        {
            using (var client = SetupClientAndSetMenu(2))
            {
                var url = ApiUriHelper.GetBaseUri() + ApiUriHelper.ComposeUrl(ApiUriHelper.RegisterClassUrl, id.ToString());
                try
                {
                    var response = await client.PostAsJsonAsync(url, id);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsByteArrayAsync().Result;
                        Session[SessionHelper.XChangeMessageSession] = String.Format("You have just successfully registered the {0} course.", courseName);
                        return RedirectToAction("registeredcourses");
                    }
                    else
                    {
                        return RedirectToAction("index", "aaserror");
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("index", "aaserror");
                }
            }
        }

        public async Task<ActionResult> DropCourse(int id, string courseName = "")
        {
            using (var client = SetupClientAndSetMenu(2))
            {
                var url = ApiUriHelper.GetBaseUri() + ApiUriHelper.ComposeUrl(ApiUriHelper.DropClassUrl, id.ToString());
                try
                {
                    var response = await client.PostAsJsonAsync(url, id);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsByteArrayAsync().Result;
                        Session[SessionHelper.XChangeMessageSession] = String.Format("You have just successfully dropped the {0} course.", courseName);
                        return RedirectToAction("registeredcourses");
                    }
                    else
                    {
                        return RedirectToAction("index", "aaserror");
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("index", "aaserror");
                }
            }
        }

        private HttpClient SetupClientAndSetMenu(int? activeMenu, int clientTimeOutInMinute = 2)
        {
            var session = SessionHelper.GetSession<UserSession>(UserSession.LoggedinUserSession);
            if (activeMenu.HasValue) session.SetMenuActive(activeMenu.Value);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("aas-accesskey", session.TokenString);
            client.Timeout = new TimeSpan(0, clientTimeOutInMinute, 0);
            return client;
        }
    }
}