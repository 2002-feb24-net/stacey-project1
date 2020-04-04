using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CornNuggets.DataAccess.Models
{
    public partial class OrderLog
    {
        public int LogId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int? ProductId { get; set; }
        [Required]
        public int? ProductQty { get; set; }
        public decimal SubTotal { get; set; }

        public virtual Orders Order { get; set; }
        public virtual Products Product { get; set; }
    }
}
