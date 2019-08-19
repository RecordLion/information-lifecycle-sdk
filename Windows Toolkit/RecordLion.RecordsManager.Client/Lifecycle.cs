using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class Lifecycle
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Notes { get; set; }

        public LifecyclePhase[] Phases { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}