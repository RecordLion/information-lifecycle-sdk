using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class ActionItem
    {
        public long Id { get; set; }

        public long RecordId { get; set; }

        public bool IsCompleted { get; set; }

        public LifecyclePhaseAction Action { get; set; }

        public bool? AutomationFailed { get; set; }

        public bool? AutomationUnsupported { get; set; }

        public DateTime RetentionExpirationDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}