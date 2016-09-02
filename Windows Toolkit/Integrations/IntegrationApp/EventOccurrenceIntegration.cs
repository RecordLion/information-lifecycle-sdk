using System;
using System.Diagnostics;
using RecordLion.RecordsManager.Client;

namespace IntegrationApp
{
    public static class EventOccurrenceIntegration
    {
        public static EventOccurrence CreatePropertyEventOccurrence(IRecordsManagerClient client, long triggerId, string propertyName, string propertyValue)
        {
            var eventOccurrence = new EventOccurrence();
            eventOccurrence.EventDate = DateTime.Now; //EventDate is when this occurrence actually happened; does not have to be DateTime.Now
            eventOccurrence.EventTriggerId = triggerId;
            eventOccurrence.TargetType = EventOccurrenceTargetType.Property;
            eventOccurrence.TargetProperty = propertyName;
            eventOccurrence.TargetValue = propertyValue;

            Debug.Assert(eventOccurrence.Id == 0, "The new event occurrence should have an ID equal to zero.");

            eventOccurrence = client.CreateEventOccurrence(eventOccurrence);

            Debug.Assert(eventOccurrence.Id > 0, "The created event occurrence should have an ID greater than zero.");

            return eventOccurrence;
        }
    }
}
