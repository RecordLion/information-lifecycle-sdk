using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class RecordCustody
    {
        public long Id { get; set; }

        public string DeliveringUsername { get; set; }

        public string CustodyUsername { get; set; }

        public DateTime CustodyDate { get; set; }
    }
}