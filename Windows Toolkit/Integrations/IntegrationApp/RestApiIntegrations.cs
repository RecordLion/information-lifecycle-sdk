using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using RecordLion.RecordsManager.Client;

namespace IntegrationApp
{
    public static class RestApiIntegrations
    {
        #region Fields

        private static readonly JsonSerializerSettings inboundJsonSettings = new JsonSerializerSettings()
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Local
        };

        private static readonly JsonSerializerSettings outboundJsonSettings = new JsonSerializerSettings()
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
        };

        #endregion


        #region Properties

        public static Uri InfoLifecycleServerUrl { get; set; }

        public static NetworkCredential Credentials { get; set; }

        #endregion


        #region Public Methods

        //***********************************************
        //                RECORD CLASSES
        //***********************************************

        public static DateTime GetRecordClassesLastEdit()
        {
            string jsonResponse = null;

            //Make HTTP GET request and populate json variable...
            using (var client = GetWebClient())
            {
                string endpoint = RestApiV1Constants.GET_RECORDCLASSES_LASTEDIT;

                jsonResponse = client.DownloadString(new Uri(endpoint, UriKind.Relative));
            }

            //Convert HTTP response to DateTime...
            DateTime lastEdit = JsonConvert.DeserializeObject<DateTime>(jsonResponse, inboundJsonSettings);

            //Remove Debug.Assert in a production environment...
            Debug.Assert(lastEdit.Kind == DateTimeKind.Local, "Invalid response! Expected a local DateTime instance.");

            return lastEdit;
        }

        //***********************************************
        //                    TRIGGERS
        //***********************************************

        public static IClientPagedItems<RetentionTrigger> SearchTriggersByTitle(string title, int page, int pageSize)
        {
            string jsonResponse = null;

            //Make HTTP GET request and populate json variable...
            using (var client = GetWebClient())
            {
                string endpoint = FormatResourceUrl(RestApiV1Constants.GET_TRIGGERS_CONTAINING_TITLE, title, page, pageSize);

                jsonResponse = client.DownloadString(new Uri(endpoint, UriKind.Relative));
            }

            return JsonConvert.DeserializeObject<ClientPagedItems<RetentionTrigger>>(jsonResponse, inboundJsonSettings);
        }

        public static RetentionTrigger GetFirstManualEventTrigger()
        {
            string jsonResponse = null;

            //Make HTTP GET request and populate json variable...
            using (var client = GetWebClient())
            {
                //In a production scenario, you would likely search for triggers by title and handle paging.
                //In this example, we use the first 100 triggers returned from the API and filter those results.
                int page = 1;
                int pageSize = 100;

                string endpoint = FormatResourceUrl(RestApiV1Constants.GET_TRIGGERS_ALL, page, pageSize);

                jsonResponse = client.DownloadString(new Uri(endpoint, UriKind.Relative));
            }

            var pagedItems = JsonConvert.DeserializeObject<ClientPagedItems<RetentionTrigger>>(jsonResponse, inboundJsonSettings);

            return pagedItems.Items
                             .Where(item => (item.TriggerType == RetentionTriggerType.Event) && (item.Recurrence == RetentionEventRecurrence.Manual))
                             .FirstOrDefault();
        }

        public static RetentionTrigger GetTriggerById(long id)
        {
            string jsonResponse = null;

            //Make HTTP GET request and populate json variable...
            using (var client = GetWebClient())
            {
                string endpoint = FormatResourceUrl(RestApiV1Constants.GET_TRIGGER_WITH_ID, id);

                jsonResponse = client.DownloadString(new Uri(endpoint, UriKind.Relative));
            }

            return JsonConvert.DeserializeObject<RetentionTrigger>(jsonResponse, inboundJsonSettings);
        }


        //***********************************************
        //                EVENT OCCURRENCES
        //***********************************************

        public static IClientPagedItems<EventOccurrence> SearchEventOccurrencesByEventTitle(string eventTitle, int page, int pageSize)
        {
            string jsonResponse = null;

            //Make HTTP GET request and populate json variable...
            using (var client = GetWebClient())
            {
                string endpoint = FormatResourceUrl(RestApiV1Constants.GET_EVENTOCCURRENCES_CONTAINING_EVENTTITLE, eventTitle, page, pageSize);

                jsonResponse = client.DownloadString(new Uri(endpoint, UriKind.Relative));
            }

            return JsonConvert.DeserializeObject<ClientPagedItems<EventOccurrence>>(jsonResponse, inboundJsonSettings);
        }

        public static EventOccurrence CreateEventOccurrence(EventOccurrence eventOccurrence)
        {
            string jsonResponse = null;

            //Make HTTP POST request and populate json variable...
            using (var client = GetWebClient())
            {
                string endpoint = RestApiV1Constants.POST_EVENTOCCURRENCE;

                string jsonRequest = JsonConvert.SerializeObject(eventOccurrence, outboundJsonSettings);

                jsonResponse = client.UploadString(new Uri(endpoint, UriKind.Relative), "POST", jsonRequest);
            }

            var result = JsonConvert.DeserializeObject<EventOccurrence>(jsonResponse, inboundJsonSettings);

            //Remove Debug.Assert in a production environment...
            Debug.Assert(result.Id != 0, "Invalid Event Occurrence! The Id should have been populated after creation.");

            return result;
        }

        #endregion


        #region Private Methods

        private static string FormatResourceUrl(string endpointBaseUrl, params object[] args)
        {
            string[] encoded = null;

            if (args != null)
            {
                encoded = new string[args.Length];

                for (int i = 0; i < args.Length; i++)
                {
                    encoded[i] = (args[i] != null) ? Uri.EscapeDataString(args[i].ToString()) : null;
                }
            }

            return string.Format(endpointBaseUrl, encoded);
        }

        private static WebClient GetWebClient()
        {
            var client = new WebClient();

            client.BaseAddress = InfoLifecycleServerUrl.ToString();

            client.Headers.Add(HttpRequestHeader.ContentType, RestApiV1Constants.HEADER_CONTENTTYPE);

            client.Headers.Add(HttpRequestHeader.Authorization, GetBasicAuthorizationHeaderValue());

            return client;
        }

        private static string GetBasicAuthorizationHeaderValue()
        {
            if (Credentials != null)
            {
                byte[] buffer = Encoding.ASCII.GetBytes($"{Credentials.UserName}:{Credentials.Password}");

                return $"Basic {Convert.ToBase64String(buffer)}";
            }

            return string.Empty;
        }

        #endregion
    }
}
