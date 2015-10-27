using System;
using System.Collections.Generic;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class RecordRequest
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Requestor { get; set; }

        public string Clerk { get; set; }

        public RecordRequestStatus Status { get; set; }

        public DateTime OpenedDate { get; set; }

        public DateTime? SubmittedDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<RecordRequestItem> Items { get; set; }
    }
}