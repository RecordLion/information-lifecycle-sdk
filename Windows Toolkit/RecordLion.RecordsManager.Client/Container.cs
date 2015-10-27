using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class Container
    {
        public Container()
        {
            this.CanContainRecords = false;
            this.ContainerType = Client.ContainerType.Location;
            this.ContainerMedium = Client.ContainerMedium.Paper;
            this.BarcodeSymbology = Client.BarcodeSymbology.Code39;
            this.BarcodeSymbologyAlt = Client.BarcodeSymbology.Code39;
        }


        public long Id { get; set; }

        public long? ParentId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Barcode { get; set; }

        public BarcodeSymbology BarcodeSymbology { get; set; }

        public string BarcodeAlt { get; set; }

        public BarcodeSymbology BarcodeSymbologyAlt { get; set; }

        public ContainerType ContainerType { get; set; }

        public string OtherContainerType { get; set; }

        public ContainerMedium ContainerMedium { get; set; }

        public string OtherContainerMedium { get; set; }

        public int? Capacity { get; set; }

        public bool CanContainRecords { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool HasChildren { get; set; }

        public int Count { get; set; }

        public long? RecordClassId { get; set; }

        public string FolderPath { get; set; }
    }
}