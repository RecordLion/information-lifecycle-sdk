using System;

namespace RecordLion.RecordsManager.Client
{
    public class InboxActionItem
    {
        public string Id { get; set; }

        public long? ActionItemId { get; set; }

        public bool? AutomationFailed { get; set; }

        public bool? AutomationUnsupported { get; set; }

        public bool? IsApproved { get; set; }

        public LifecyclePhaseAction PhaseAction { get; set; }

        public int PhaseOrder { get; set; }

        public bool RequireApproval { get; set; }

        public long? RecordId { get; set; }

        public string RecordUri { get; set; }

        public string RecordTitle { get; set; }

        public string RecordIdentifier { get; set; }

        public string RecordDescription { get; set; }     

        public long? CaseFileId { get; set; }

        public string CaseFileTitle { get; set; }

        public string CaseFileCode { get; set; }

        public long RecordClassId { get; set; }

        public string RecordClassTitle { get; set; }

        public string RecordClassCode { get; set; }

        public bool IsCaseFile { get; set; }

        public DateTime RetentionExpirationDate { get; set; }
    }
}
