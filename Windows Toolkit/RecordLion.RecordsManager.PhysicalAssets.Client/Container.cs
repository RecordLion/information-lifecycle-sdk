using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLion.RecordsManager.PhysicalAssets.Client
{
    public class Container
    {
        public Container()
        {
            this.CanContainAssets = false;
            this.NodeType = NodeType.Location;
            this.LocationType = Client.LocationType.Folder;
            this.BarcodeSymbology = BarcodeSymbology.Code39;
            this.BarcodeSymbologyAlt = BarcodeSymbology.Code39;
        }

        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Subject { get; set; }

        public string Keywords { get; set; }

        public NodeType NodeType { get; set; }

        public LocationType? LocationType { get; set; }

        public int? Capacity { get; set; }

        public bool CanContainAssets { get; set; }

        public string Barcode { get; set; }

        public BarcodeSymbology BarcodeSymbology { get; set; }

        public string BarcodeAlt { get; set; }

        public BarcodeSymbology BarcodeSymbologyAlt { get; set; }

        public bool AllowRequests { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool UniquePermissions { get; set; }

        public bool HasChildren { get; internal set; }

        public int Count { get; internal set; }

        public bool HasCustomMetadata { get; protected set; }
    }
}
