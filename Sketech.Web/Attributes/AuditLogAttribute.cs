using Sketech.Infrastructure.Ioc;
using Sketech.Infrastructure.Logging;
using System.Collections.Generic;
using System.Web.Http.Filters;

namespace Sketech.Web.ActionFilters
{
    public class AuditLogAttribute : ActionFilterAttribute
    {
        private string _eventName;
        private string _detail;
        private Dictionary<string, string> _properties;

        public AuditLogAttribute(string eventName, string detail, Dictionary<string, string> properties = null): base()
        {
            _eventName = eventName;
            _detail = detail;
            _properties = properties;
        }

        public AuditLogAttribute(string eventName, string detail) : this(eventName, detail, null)
        {
        }

        public AuditLogAttribute(string eventName) : this(eventName, null, null)
        {
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var logger = SkServiceLocator.Get<SkLoggerBase>();
            logger.LogAudit(_eventName, _detail, _properties);
        }
    }
}