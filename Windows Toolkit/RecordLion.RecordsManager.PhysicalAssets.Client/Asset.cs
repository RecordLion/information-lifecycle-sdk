using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLion.RecordsManager.PhysicalAssets.Client
{
    public class Asset
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Subject { get; set; }

        public string Keywords { get; set; }

        public long? HomeLocationId { get; set; }

        public Location HomeLocation { get; set; }

        public AssetType AssetType { get; set; }

        public string OtherAssetType { get; set; }

        public AssetFormat Format { get; set; }

        public string OtherFormat { get; set; }

        public string Owner { get; set; }

        public string Barcode { get; set; }

        public BarcodeSymbology BarcodeSymbology { get; set; }

        public string BarcodeAlt { get; set; }

        public BarcodeSymbology BarcodeSymbologyAlt { get; set; }

        public bool AllowRequests { get; set; }

        public string TemporaryLocation { get; set; }

        public long? CurrentLocationId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public DateTime? LockedDate { get; internal set; }

        public string LockUser { get; internal set; }

        public DateTime? ChargeOutDate { get; set; }

        public string ChargeOutUser { get; set; }

        public bool IsChildAsset { get; set; }
    }
}
