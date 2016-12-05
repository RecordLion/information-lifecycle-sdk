using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class RuleSet
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public Rule[] Rules { get; set; }

        public bool HasRules
        {
            get
            {
                return this.Rules != null &&
                       this.Rules.Length > 0;
            }
        }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}