using System;
using System.Linq;
using RecordLion.RecordsManager.Client;

namespace IntegrationApp
{
    public static class RetentionTriggerIntegrations
    {
        public static RetentionTrigger GetTriggerById(IRecordsManagerClient client, long id)
        {
            return client.GetTrigger(id);
        }

        public static RetentionTrigger GetTriggerByTitle(IRecordsManagerClient client, string title)
        {
            //Get Trigger By Title
            //Searching for triggers by Title, using a case-insensitive comparison on the returned results

            int page = 1;
            int pageSize = 10;
            bool hasMorePages = false;
            StringComparison caseComparison = StringComparison.OrdinalIgnoreCase; //Ignore casing
            IClientPagedItems<RetentionTrigger> pagedTriggers = null;
            RetentionTrigger trigger = null;

            do
            {
                pagedTriggers = client.SearchTriggers(title, page, pageSize);

                hasMorePages = page++ < pagedTriggers.PageCount;

                trigger = pagedTriggers.Items
                                       .Where(item => (item.TriggerType == RetentionTriggerType.Event) &&
                                             (item.Recurrence == RetentionEventRecurrence.Manual) &&
                                             (item.Title.Equals(title, caseComparison)))
                                       .FirstOrDefault();

            }
            while (hasMorePages && (trigger == null));

            return trigger;
        }

        public static RetentionTrigger GetFirstManualEventTrigger(IRecordsManagerClient client)
        {
            //Illustrates the following:
            //*Paging results by using IClientPagedItems
            //*Filtering results to a specific type of trigger

            int page = 1;
            int pageSize = 10;
            bool hasMorePages = false;
            IClientPagedItems<RetentionTrigger> pagedTriggers = null;
            RetentionTrigger trigger = null;

            do
            {
                pagedTriggers = client.GetTriggers(page, pageSize);

                hasMorePages = page++ < pagedTriggers.PageCount;

                trigger = pagedTriggers.Items
                                       .Where(item => (item.TriggerType == RetentionTriggerType.Event) && (item.Recurrence == RetentionEventRecurrence.Manual))
                                       .FirstOrDefault();
            } while (hasMorePages && (trigger == null));

            return trigger;
        }
    }
}
