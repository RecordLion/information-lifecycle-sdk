using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordLion.RecordsManager.Client
{
    public class ManagedProperty
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string[] Names { get; set; }
    }
}
