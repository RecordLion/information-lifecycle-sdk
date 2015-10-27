using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class EventOccurrence
    {
        public long Id { get; set; }

        public DateTime EventDate { get; set; }

        public int EventTriggerId { get; set; }

        public EventOccurrenceTargetType TargetType { get; set; }

        public string TargetProperty { get; set; }

        public string TargetValue { get; set; }

        public long? CaseFileId { get; set; }
    }
}