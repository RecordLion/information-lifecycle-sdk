using System;
using RecordLion.RecordsManager.Client;

namespace IntegrationApp
{
    internal static class ProgramRestApi
    {
        public static void Execute(string[] args)
        {
            Console.WriteLine("Please enter the URL for your Information Lifecycle server...");

            string url = Console.ReadLine();

            Console.WriteLine("Please enter the service account User Name...");

            string user = Console.ReadLine();

            Console.WriteLine("Please enter the server account Password (WARNING: pwd will be displayed)...");

            string pwd = Console.ReadLine();

            SearchTriggersByTitle(url, user, pwd);
        }

        private static void GetTriggerById(string url, string user, string pwd)
        {
            RestApiIntegrations.Credentials = new System.Net.NetworkCredential(user, pwd);

            RestApiIntegrations.InfoLifecycleServerUrl = new Uri(url, UriKind.Absolute);

            long triggerId = 13;

            RetentionTrigger trigger = RestApiIntegrations.GetTriggerById(triggerId);
        }

        private static void SearchTriggersByTitle(string url, string user, string pwd)
        {
            RestApiIntegrations.Credentials = new System.Net.NetworkCredential(user, pwd);

            RestApiIntegrations.InfoLifecycleServerUrl = new Uri(url, UriKind.Absolute);

            string title = "Terminate Employee";
            int page = 1;
            int pageSize = 100;

            IClientPagedItems<RetentionTrigger> triggers = RestApiIntegrations.SearchTriggersByTitle(title, page, pageSize);
        }

        private static void GetRecordClassLastEditTime(string url, string user, string pwd)
        {
            RestApiIntegrations.Credentials = new System.Net.NetworkCredential(user, pwd);

            RestApiIntegrations.InfoLifecycleServerUrl = new Uri(url, UriKind.Absolute);

            DateTime lastEdit = RestApiIntegrations.GetRecordClassesLastEdit();
        }

        private static void CreateEventOccurrence(string url, string user, string pwd)
        {
            RestApiIntegrations.Credentials = new System.Net.NetworkCredential(user, pwd);

            RestApiIntegrations.InfoLifecycleServerUrl = new Uri(url, UriKind.Absolute);

            RetentionTrigger trigger = RestApiIntegrations.GetFirstManualEventTrigger();

            EventOccurrence eventOccurrence = new EventOccurrence();
            eventOccurrence.EventDate = DateTime.SpecifyKind(DateTime.Parse("1/15/2015"), DateTimeKind.Local);
            eventOccurrence.TargetType = EventOccurrenceTargetType.Property;
            eventOccurrence.TargetProperty = "EmployeeId";
            eventOccurrence.TargetValue = "12345";
            eventOccurrence.EventTriggerId = trigger.Id;

            eventOccurrence = RestApiIntegrations.CreateEventOccurrence(eventOccurrence);
        }

        private static void SearchEventOccurrences(string url, string user, string pwd)
        {
            RestApiIntegrations.Credentials = new System.Net.NetworkCredential(user, pwd);

            RestApiIntegrations.InfoLifecycleServerUrl = new Uri(url, UriKind.Absolute);

            string eventTitle = "Tax Return";
            int page = 1;
            int pageSize = 100;

            IClientPagedItems<EventOccurrence> pagedItems = RestApiIntegrations.SearchEventOccurrencesByEventTitle(eventTitle, page, pageSize);
        }
    }
}
