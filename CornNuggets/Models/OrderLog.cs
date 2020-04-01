using System;
using System.Collections.Generic;

namespace CornNuggets.WebUI.Models
{
    public partial class OrderLog
    {
        public int LogId { get; set; }
        public int OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? ProductQty { get; set; }
        public decimal SubTotal { get; set; }

        public virtual Orders Order { get; set; }
        public virtual Products Product { get; set; }
    }
}
