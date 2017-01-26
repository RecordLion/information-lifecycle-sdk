using System;
using RecordLion.RecordsManager.Client;

namespace IntegrationApp
{
    internal static class ProgramClientObjectModel
    {
        public static void Execute(string[] args)
        {
            Console.WriteLine("Please enter the URL for your Information Lifecycle server...");

            string url = Console.ReadLine();

            CreateEventOccurrence(url);
        }

        private static void CreateEventOccurrence(string url)
        {
            //Create a new client using default network credentials
            IRecordsManagerClient client = RecordsManagerClientFactory.Create(url);

            //Currently for a production scenario, you would need to get the retention trigger by ID
            //This sample finds the first manual, event-based, retention trigger and uses that instead.
            RetentionTrigger retentionTrigger = RetentionTriggerIntegrations.GetFirstManualEventTrigger(client);

            //Create an event occurrence for the specified trigger using the specified property name & value
            EventOccurrence eventOccurrence = EventOccurrenceIntegration.CreatePropertyEventOccurrence(client, retentionTrigger.Id, "LoanNumber", "12345");
        }
    }
}
