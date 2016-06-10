using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public enum RetentionTriggerType
    {
        Event,
        DateProperty, 
        Rule,
        Special
    }
}