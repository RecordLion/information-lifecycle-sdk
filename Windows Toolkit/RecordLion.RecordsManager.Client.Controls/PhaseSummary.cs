using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client.Controls
{
    public class PhaseSummary
    {
        public string DisplayOrder { get; set; }

        public string Summary { get; set; }

        public bool IsCurrent { get; set; }

        public bool IsCompleted { get; set; }
    }
}