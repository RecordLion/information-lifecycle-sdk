using System;
using System.Collections.Generic;

namespace RecordLion.RecordsManager.PhysicalAssets.Client
{
    public class CustomMetadata
    {
        public int Id { get; set; }

        public CustomMetadataType? CustomMetadataType { get; set; }

        public string Title { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public IEnumerable<CustomMetadataChoice> CustomMetadataChoices { get; set; }
    }
}
