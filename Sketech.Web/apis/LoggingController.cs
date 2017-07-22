using Sketech.Dals.Logging;
using Sketech.Entities.Logging;
using Sketech.Infrastructure.Ioc;
using Sketech.Infrastructure.Logging;
using Sketech.Web.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sketech.Web.apis
{
    public class LoggingController : ApiController
    {
        [HttpGet]
        [SkAuthorize]
        public async Task<IList<AuditLogEntry>> GetAuditLogs(int skip = 0, int take = 100)
        {
            IList<AuditLogEntry> logs = null;
            var logger = SkServiceLocator.Get<SkLoggerBase>() as SkDbLogger;
            if (logger != null)
            {
                logs = await logger.GetAuditLogs();
            }
            return logs;
        }
    }
}
