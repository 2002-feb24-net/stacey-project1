using CornNuggets.DataAccess.Models;
using System.Collections.Generic;

namespace CornNuggets.DataAccess.Repositories
{
    public interface ICornNuggetsRepository
    {
        public Customers AddCustomer(Customers newCustomer);
        public IEnumerable<Customers> SearchCustomers(string fname, string lname);
        public IEnumerable<NuggetStores> GetStoreOrders(int storeid);
        public IEnumerable<Orders> NewOrder(string fname, string lname, int prodid, int prodqty);
        public IEnumerable<Orders> AddToOrder(int orderid, int prodid, int prodqty);

        public IEnumerable<Orders> GetCustomerOrders(int customerid);
        public IEnumerable<OrderLog> GetOrdersDetails(int orderid);
    }
}