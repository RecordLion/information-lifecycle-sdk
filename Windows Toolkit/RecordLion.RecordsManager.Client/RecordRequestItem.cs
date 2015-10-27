using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class RecordRequestItem
    {
        public long Id { get; set; }

        public long RecordId { get; set; }

        public RecordRequestItemStatus Status { get; set; }
    }
}