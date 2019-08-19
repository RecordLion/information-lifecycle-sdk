using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordLion.RecordsManager.Client
{
    public class RecordProperty
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public RecordizePropertyState State { get; set; } = RecordizePropertyState.Default;

        public long RecordId { get; set; }
    }
}
