using Sketech.Dals.Logging;
using Sketech.Entities.Logging;
using Sketech.Infrastructure.Ioc;
using Sketech.Infrastructure.Logging;
using Sketech.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sketech.Web.Controllers
{
    [SkAuthorize]
    public class HomeController : Controller
    {
        [SkAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<ActionResult> AuditLogMvc()
        {
            ViewBag.Message = "Audit Log with MVC";

            IList<AuditLogEntry> logs = null;
            var logger = SkServiceLocator.Get<SkLoggerBase>() as SkDbLogger;
            if(logger != null)
            {
                logs = await logger.GetAuditLogs();
            }

            return View(logs);
        }

        public ActionResult AuditLogVue()
        {
            ViewBag.Message = "Audit Log with MVC";

            return View();
        }
    }
}