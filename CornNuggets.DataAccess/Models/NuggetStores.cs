using System;
using System.Collections.Generic;

namespace CornNuggets.DataAccess.Models
{
    public partial class NuggetStores
    {
        public NuggetStores()
        {
            Orders = new HashSet<Orders>();
        }

        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string StoreLocation { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
