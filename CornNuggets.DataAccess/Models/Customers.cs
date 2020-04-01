using CornNuggets.DataAccess.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CornNuggets.DataAccess.Models
{
    public partial class Customers
    {
        public Customers()
        {
            Orders = new HashSet<Orders>();
        }

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PreferredStore { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
