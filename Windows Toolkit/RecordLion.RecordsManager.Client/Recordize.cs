using System;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class Recordize
    {
        public Recordize()
        {
            this.State = RecordState.NewOrModified;
        }


        public string Uri { get; set; }

        public string PreviousUri { get; set; }

        public long? RecordClassId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool? IsManuallyClassified { get; set; }
        
        public DateTime? OriginatedDate { get; set; }

        public RecordState State { get; set; }

        public bool? IsObsolete { get; set; }

        public bool? IsSuperseded { get; set; }

        public bool? IsRecord { get; set; }        

        public bool? IsVital { get; set; }

        public bool? IsPhysical { get; set; }

        public RecordizeProperty[] Properties { get; set; }
    }
}