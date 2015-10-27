using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class Retention
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Authority { get; set; }

        public long TriggerId { get; set; }

        public RetentionTimePeriod TimePeriod { get; set; }

        public int Interval { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}