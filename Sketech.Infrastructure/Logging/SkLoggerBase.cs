using System;
using System.Collections.Generic;

namespace Sketech.Infrastructure.Logging
{
    public abstract class SkLoggerBase
    {
        protected abstract void LogSystemEvent(SkLogLevel logLevel, string message, string detail);
        
        protected abstract void LogAuditEvent(string eventName, Dictionary<string, string> props);

        public void LogException(Exception ex)
        {
            LogSystemEvent(SkLogLevel.Error, ex.Message, ex.ToString());
        }

        public void LogWarning(string message, string detail)
        {
            LogSystemEvent(SkLogLevel.Warning, message, detail);
        }

        public void LogInfo(string message, string detail)
        {
            LogSystemEvent(SkLogLevel.Info, message, detail);
        }

        public void LogAudit(string eventName, Dictionary<string, string> props)
        {
            LogAuditEvent(eventName, props);
        }
    }
}
