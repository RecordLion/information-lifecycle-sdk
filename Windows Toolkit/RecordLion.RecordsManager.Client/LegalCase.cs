using System;

namespace RecordLion.RecordsManager.Client
{
    public class LegalCase
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Court { get; set; }

        public string CaseNumber { get; set; }

        public DateTime? OpenedDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public Rule[] HoldRules { get; set; }

        public bool IsOpen
        {
            get
            {
                return this.OpenedDate.HasValue && this.OpenedDate.Value < DateTime.Now && (!this.ClosedDate.HasValue || this.ClosedDate.Value > DateTime.Now);
            }
        }
    }
}