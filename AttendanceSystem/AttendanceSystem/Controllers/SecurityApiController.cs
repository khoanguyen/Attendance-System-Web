using AttendanceSystem.Infrastructure.Filters;
using AttendanceSystem.Infrastructure.Security;
using AttendanceSystem.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AttendanceSystem.Controllers
{
    [RoutePrefix("api/security")]
    public class SecurityApiController : AASApiController
    {

        [HttpPost, Route("login")]
        public IHttpActionResult Login([FromBody]LoginLogicModel model)
        {
            var result = Logic.SignIn(model);
            return JsonEx(result);
        }

        [HttpPost, Route("exchange"), TokenExchangeFilter]
        public IHttpActionResult Exchange()
        {
            var tokenString = Token.ReadTokenStringFromHeader(this.Request);
            return JsonEx(Logic.ExchangeToken(tokenString));
        }
    }
}