using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLion.RecordsManager.PhysicalAssets.Client
{
    public class AssetCustomMetadata
    {
        public Guid AssetId { get; set; }

        public long CustomMetadataId { get; set; }

        public string Value { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
