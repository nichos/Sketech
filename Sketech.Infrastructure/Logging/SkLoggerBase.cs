using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sketech.Infrastructure.Logging
{
    public abstract class SkLoggerBase
    {
        protected abstract Task LogSystemEventAsync(SkLogLevel logLevel, string message, string detail);
        
        protected abstract Task LogAuditEventAsync(string eventName, string detail, Dictionary<string, string> props);

        public void LogError(Exception ex)
        {
            Task.Run(()=> LogSystemEventAsync(SkLogLevel.Error, ex.Message, ex.ToString())).Wait();
        }

        public void LogWarning(string message, string detail)
        {
            Task.Run(() => LogSystemEventAsync(SkLogLevel.Warning, message, detail)).Wait();
        }

        public void LogInfo(string message, string detail)
        {
            Task.Run(() => LogSystemEventAsync(SkLogLevel.Info, message, detail)).Wait();
        }

        public void LogAudit(string eventName, string detail, Dictionary<string, string> props)
        {
            Task.Run(() => LogAuditEventAsync(eventName, detail, props)).Wait();
        }
    }
}
