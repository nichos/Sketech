using System;
using System.Collections.Generic;
using Sketech.Infrastructure.Logging;

namespace Sketech.Dals.Logging
{
    public class SkDbLogger : SkLoggerBase
    {
        protected override void LogAuditEvent(string eventName, Dictionary<string, string> props)
        {
            throw new NotImplementedException();
        }

        protected override void LogSystemEvent(SkLogLevel logLevel, string message, string detail)
        {
            throw new NotImplementedException();
        }
    }
}
