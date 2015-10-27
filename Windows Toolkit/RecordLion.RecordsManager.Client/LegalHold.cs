using System;

namespace RecordLion.RecordsManager.Client
{
    public class LegalHold
    {
        public long Id { get; set; }

        public string Uri { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public long LegalCaseId { get; set; }

        public string LegalCaseTitle { get; set; }

        public string LegalCaseNumber { get; set; }
    }
}