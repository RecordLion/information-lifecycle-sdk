using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLion.RecordsManager.PhysicalAssets.Client
{
    public class ChargeOutRequest : ChargeRequest
    {
        public DateTime RequestDate { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? ExtensionDate { get; set; }

        public long LocationId { get; set; }
    }
}
