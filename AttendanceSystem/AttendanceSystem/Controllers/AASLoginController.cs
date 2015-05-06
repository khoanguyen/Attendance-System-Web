using AttendanceSystem.Infrastructure.Errors;
using AttendanceSystem.Infrastructure.Filters;
using AttendanceSystem.Infrastructure.Utils;
using AttendanceSystem.Models;
using AttendanceSystem.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AttendanceSystem.Controllers
{
    public class AASLoginController : Controller
    {
        // GET: AASLogin
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string userName, string userPwd, string btnStudent)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.Timeout = new TimeSpan(0, 2, 0);
                    var req = new HttpRequestMessage(HttpMethod.Post, ApiUriHelper.GetBaseUri() + "security/login");
                    var loginType = (btnStudent == null) ? "AdminLogin" : "StudentLogin";
                    req.Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                                    {
                                        new KeyValuePair<string,string>("username", userName),
                                        new KeyValuePair<string,string>("password", userPwd),
                                        new KeyValuePair<string,string>("requestType", loginType)
                                    });
                    var respond = await client.SendAsync(req);
                    var token = respond.Content.ReadAsStringAsync().Result.Trim('"');

                    if (respond.IsSuccessStatusCode)
                    {
                        UserSession session = new UserSession();
                        var isAdmin = (btnStudent==null);
                        session.SetUser(token, isAdmin);
                        respond.Dispose();
                        return (loginType == "AdminLogin" ? RedirectToAction("courses", "aasadmin") :
                                                            RedirectToAction("index", "aasstudent"));
                    }
                    else
                    {
                        var error = new ErrorMessageModel()
                        {
                            Error = "Login fail",
                            ErrorMessage = "username or password invalid"
                        };
                        return View("Index", error);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                var error = new ErrorMessageModel()
                {
                    Error = "Login fail",
                    ErrorMessage = ex.Message
                };
                return View("Index", error);
            }
            
        }

        [UserSessionFilter(UserType = LoginRequestType.None)]
        public ViewResult ChangePassword()
        {
            return View();
        }

        [HttpPost, UserSessionFilter(UserType = LoginRequestType.None)]
        public async Task<ActionResult> ChangePassword(LoginLogicModel model)
        {
            return View();
        }

        public ActionResult Logout()
        {
            SessionHelper.ClearAllSession();
            return RedirectToAction("Index");
        }
    }
}
