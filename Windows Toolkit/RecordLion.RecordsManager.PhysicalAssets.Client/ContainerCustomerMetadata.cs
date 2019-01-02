using System;

namespace RecordLion.RecordsManager.PhysicalAssets.Client
{
    public class ContainerCustomMetadata
    {
        public Guid ContainerId { get; set; }

        public long CustomMetadataId { get; set; }

        public string Value { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
