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

        public static RetentionTrigger GetFirstManualEventTrigger(IRecordsManagerClient client)
        {
            //Illustrates how to use IClientPagedItems and filter for a specific type of trigger...

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
