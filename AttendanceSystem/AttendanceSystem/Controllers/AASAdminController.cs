using AttendanceSystem.Infrastructure.Filters;
using AttendanceSystem.Infrastructure.Utils;
using AttendanceSystem.Models;
using AttendanceSystem.Models.LogicModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AttendanceSystem.Controllers
{
    [UserSessionFilterAttribute(UserType = LoginRequestType.AdminLogin)]
    public partial class AASAdminController : Controller
    {
        public async Task<ActionResult> Courses()
        {
            using (var client = SetupClientAndSetMenu(1))
            {
                var url = ApiUriHelper.GetBaseUri() + ApiUriHelper.allClassesUrl;
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    response.Dispose();                    
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var courses = serializer.Deserialize<List<ClassLogicModel>>(data.Replace("__type", "TypeName"));
                    return View("Courses", courses);
                }
            }
            return View();
        }

        
        public async Task<ActionResult> Course(int id)
        {
            using (var client = SetupClientAndSetMenu(1))
            {
                var url = ApiUriHelper.GetBaseUri() + ApiUriHelper.ComposeUrl(ApiUriHelper.classByIdUrl, id.ToString());                
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    response.Dispose();
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var course = serializer.Deserialize<ClassLogicModel>(result.Replace("__type", "TypeName"));
                    return View(course);
                }
                return Redirect("/aaserror");
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            using (var client = SetupClientAndSetMenu(1))
            {
                var url = ApiUriHelper.GetBaseUri() + ApiUriHelper.ComposeUrl(ApiUriHelper.classByIdUrl, id.ToString());
                try
                {
                    var response = await client.DeleteAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { message = "Delete successful" });
                    }
                    else
                    {
                        throw new HttpException(500, "Delete fail");
                    }
                }
                catch (Exception ex)
                {
                    throw new HttpException(500, ex.Message);
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