using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sketech.Web.apis
{
    [Authorize]
    public class CustomerController : ApiController
    {
        [HttpGet]
        public string Ping()
        {
            var curUser = User.Identity.Name;
            return curUser;
        }
    }
}
