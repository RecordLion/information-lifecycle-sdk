using System.Collections.Generic;
using System.Linq;

namespace RecordLion.RecordsManager.Client
{
    public class ClientPagedItems<T> : IClientPagedItems<T>
    {
        public ClientPagedItems()
        {
        }


        public int Page { get; set; }

        public int PageCount { get; set; }

        public int ItemCount
        {
            get
            {
                return (this.Items == null) ? 0 : this.Items.Count();
            }
        }


        public IEnumerable<T> Items { get; set; }
    }
}