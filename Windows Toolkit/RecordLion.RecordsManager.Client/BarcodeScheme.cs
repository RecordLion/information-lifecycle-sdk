using System;

namespace RecordLion.RecordsManager.Client
{
    public class BarcodeScheme
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Prefix { get; set; }

        public string Suffix { get; set; }

        public long StartRange { get; set; }

        public long EndRange { get; set; }

        public long Counter { get; set; }

        public int CounterPadding { get; set; }

        public DateTime? OpenedDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsCountFull
        {
            get
            {
                if (this.EndRange > 0)
                {
                    return this.Counter >= this.EndRange;
                }

                return false;
            }
        }


        public long CountAvailable
        {
            get
            {
                if (this.EndRange > 0)
                {
                    if (this.Counter >= this.EndRange)
                    {
                        return 0;
                    }

                    return this.EndRange - this.Counter;
                }

                return long.MaxValue - this.Counter;
            }
        }
    }
}