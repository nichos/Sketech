using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sketech.Entities.Logging
{
    public class AuditLogEntry
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string EventName { get; set; }
        public string EventDetail { get; set; }
        public string ActionUser { get; set; }
    }
}
