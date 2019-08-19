using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class LifecyclePhase
    {
        public long Id { get; set; }

        public int Order { get; set; }

        public long RetentionId { get; set; }

        public LifecyclePhaseAction Action { get; set; }

        public LifecyclePhaseAutomationLevel AutomationLevel { get; set; }

        public bool RequireApproval { get; set; }

        public int RepeatApprovalInterval { get; set; }

        public LifecyclePhaseRepeatApprovalTimePeriod RepeatApprovalTimePeriod { get; set; }
    }
}