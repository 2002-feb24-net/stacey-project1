using System;
using System.Collections.Generic;

namespace CornNuggets.DataAccess.Models
{
    public partial class Products
    {
        public Products()
        {
            OrderLog = new HashSet<OrderLog>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public int Inventory { get; set; }

        public virtual ICollection<OrderLog> OrderLog { get; set; }
    }
}
