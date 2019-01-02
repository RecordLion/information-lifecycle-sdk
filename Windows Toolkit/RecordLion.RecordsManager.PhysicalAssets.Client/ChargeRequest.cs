using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLion.RecordsManager.PhysicalAssets.Client
{
    public abstract class ChargeRequest
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string RequestUser { get; set; }

        public string ProcessingUser { get; set; }

        public DateTime? ProcessedDate { get; set; }

        public string Reason { get; set; }

        public string RejectionReason { get; set; }

        public bool Urgent { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public DateTime? SubmissionDate { get; set; }

        public ChargeRequestStatus Status { get; set; }

        public string Notes { get; set; }
    }
}
