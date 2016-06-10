using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class RecordizeProperty
    {
        public RecordizeProperty()
            : this(null, null)
        {
        }

        public RecordizeProperty(string key, string value)
            : this(key, value, RecordizePropertyState.Default)
        {

        }

        public RecordizeProperty(string key, string value, RecordizePropertyState state)
        {
            this.Key = key;
            this.Value = value;
            this.State = state;
        }


        public string Key { get; set; }

        public string Value { get; set; }

        public RecordizePropertyState State { get; set; }
    }
}