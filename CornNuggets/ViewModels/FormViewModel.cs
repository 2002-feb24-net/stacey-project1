using CornNuggets.DataAccess.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CornNuggets.DataAccess.Models
{
    public class FormViewModel : IEnumerable
    {
        public Customers Customer { get; set; }
        public Products Products { get; set; }
        public Orders Order { get; set; }
        public List<OrderLog> Items { get; set; }

        public CornNuggetsRepository Repo;
        public FormViewModel()
        {
            Customer = new Customers();
            Products = new Products();
            Order = new Orders();
            Items = new List<OrderLog>();
            Repo = new CornNuggetsRepository();
        }
        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }


    }
}
