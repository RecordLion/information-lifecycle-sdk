using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class RuleAssignment
    {
        public long Id { get; set; }

        public int Order { get; set; }

        public string Property { get; set; }

        public RuleOperator Operator { get; set; }

        public string Value { get; set; }

        public RuleDataType DataType { get; set; }

        public bool IsCaseSensitive { get; set; }

        public RuleJoin Join { get; set; }

        public long? RuleSetRefId { get; set; }

        public long? RuleRefId { get; set; }

        public bool SystemRule { get; set; }
    }
}