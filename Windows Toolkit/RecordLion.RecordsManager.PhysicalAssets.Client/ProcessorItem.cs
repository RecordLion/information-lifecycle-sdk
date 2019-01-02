using System;

namespace RecordLion.RecordsManager.PhysicalAssets.Client
{
    public class ProcessorItem
    {
        public long Id { get; set; }

        public DateTime SubmissionDate { get; set; }

        public string ProcessingUser { get; set; }

        public DateTime? ProcessedDate { get; set; }

        public long ChargeRequestId { get; set; }

        public ProcessorItemStatus Status { get; set; }

        public string Reason { get; set; }
    }

}
