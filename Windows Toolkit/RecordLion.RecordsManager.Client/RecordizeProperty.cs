using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class RecordizeProperty
    {
        public RecordizeProperty()
        {
        }


        public RecordizeProperty(string key, string value)
        {
            this.Key = key;

            this.Value = value;
        }


        public string Key { get; set; }

        public string Value { get; set; }
    }
}