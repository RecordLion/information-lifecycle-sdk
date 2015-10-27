using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class Record
    {
        public long Id { get; set; }

        public string Identifier { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public long RecordClassId { get; set; }

        public string RecordClassTitle { get; set; }

        public string RecordClassCode { get; set; }

        public string RecordClassDescription { get; set; }

        public bool IsManuallyClassified { get; set; }

        public string Uri { get; set; }

        public string LifecycleTitle { get; set; }

        public long LifecycleId { get; set; }

        public int? CurrentPhase { get; set; }

        public string CurrentPhaseAction { get; set; }

        public string CurrentPhaseRetentionTitle { get; set; }

        public string CurrentPhaseRetentionAuthority { get; set; }

        public string CustodyUser { get; set; }

        public string CustodyDeliveryingUser { get; set; }

        public DateTime? CustodyDate { get; set; }

        public bool IsObsolete { get; set; }

        public bool IsSuperseded{ get; set; }

        public bool IsRecord { get; set; }

        public bool IsVital { get; set; }

        public bool IsPermanent { get; set; }

        public long? ContainerId { get; set; }

        public string Barcode { get; set; }

        public string BarcodeAlt { get; set; }

        public DateTime? RetentionExpirationDate { get; set; }

        public DateTime? OriginatedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
        
        public bool IsPhysical
        {
            get
            {
                return this.ContainerId.HasValue;
            }
        }
    }
}