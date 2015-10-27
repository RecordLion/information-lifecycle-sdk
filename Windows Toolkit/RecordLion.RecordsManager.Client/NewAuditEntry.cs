using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class NewAuditEntry
    {
        public AuditEvent Event { get; set; }

        public string RecordUri { get; set; }

        public string Details { get; set; }

        public string User { get; set; }

        public DateTime? EntryDate { get; set; }
    }
}