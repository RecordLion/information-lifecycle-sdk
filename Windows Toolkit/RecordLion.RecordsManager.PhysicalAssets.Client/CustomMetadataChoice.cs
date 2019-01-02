using System;

namespace RecordLion.RecordsManager.PhysicalAssets.Client
{
    public class CustomMetadataChoice
    {
        public int Id { get; set; }

        public int CustomMetadataId { get; set; }

        public string Value { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
