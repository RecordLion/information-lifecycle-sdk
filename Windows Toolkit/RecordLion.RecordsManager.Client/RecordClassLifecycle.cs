using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class RecordClassLifecycle
    {
        public long Id { get; set; }

        public long RecordClassId { get; set; }

        public long LifecycleId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}