using System;
using System.Collections.Generic;

namespace CornNuggets.WebUI.Models
{
    public partial class Orders
    {
        public Orders()
        {
            OrderLog = new HashSet<OrderLog>();
        }

        public int OrderId { get; set; }
        public DateTime? DateTimeStamp { get; set; }
        public int? StoreId { get; set; }
        public int? CustomerId { get; set; }
        public decimal Total { get; set; }

        public virtual Customers Customer { get; set; }
        public virtual NuggetStores Store { get; set; }
        public virtual ICollection<OrderLog> OrderLog { get; set; }
    }
}
