using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class AuditEntry
    {
        public long Id { get; set; }

        public AuditSource Source { get; set; }

        public AuditEvent Event { get; set; }

        public AuditTarget Target { get; set; }

        public string TargetId { get; set; }

        public string Details { get; set; }

        public string User { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}