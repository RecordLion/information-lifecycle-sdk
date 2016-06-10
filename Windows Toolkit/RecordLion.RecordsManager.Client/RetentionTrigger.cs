using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class RetentionTrigger
    {
        //Common Properties
        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public RetentionTriggerType TriggerType { get; set; }


        //Date Property Trigger Properties
        public string PropertyName { get; set; }


        //Event Trigger Properties
        public DateTime? NextEventDate { get; set; }

        public RetentionEventRecurrence Recurrence { get; set; }

        public RetentionEventPosition AssignmentPosition { get; set; }


        //Rule Trigger Properties
        public Rule[] TriggerRules { get; set; }


        //Special Trigger Properties
        public RetentionTriggerSpecialType SpecialType { get; set; }
    }
}