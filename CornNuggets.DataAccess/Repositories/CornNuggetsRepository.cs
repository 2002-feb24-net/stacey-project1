using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CornNuggets.DataAccess.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CornNuggets.DataAccess.Repositories
{
    class CornNuggetsRepository : ICornNuggetsRepository
    {

        private readonly CornNuggetsContext _context;
        public CornNuggetsRepository(CornNuggetsContext context)
        {
            _context = context;
        }
        
        
        public Customers AddCustomer(Customers newCustomer)
        {
            _context.Customers.Add(newCustomer);
            _context.SaveChanges();
            return newCustomer;
        }

        public NuggetStores AddNuggetStore(NuggetStores newStore)
        {
            _context.NuggetStores.Add(newStore);
            _context.SaveChanges();
            return newStore;
        }

        public IEnumerable<Customers> SearchCustomers(string fname, string lname)
        {
            using (var context = new CornNuggetsContext())
            {

                var cust = context.Customers
                    .FromSqlRaw("EXECUTE dbo.spCustomer_GetByFullName {0}, {1}", fname, lname)
                    .ToList();
                return cust;
                
            }
        }

        public IEnumerable<NuggetStores> GetStoreOrders(int storeid)
        {
            using (var context = new CornNuggetsContext())
            {

                var stores = context.NuggetStores
                    .FromSqlRaw("EXECUTE dbo.spCustomer_GetAllByStore {0}", storeid)
                    .ToList();
                return stores;
                
            }
        }

        public IEnumerable<Orders> NewOrder(string fname, string lname, int prodid, int prodqty)
        {
            using (var context = new CornNuggetsContext())
            {

                var orders = context.Orders
                    .FromSqlRaw("EXECUTE dbo.spOrders_PlaceToStoreForCustomer {0}, {1}, {2}, {3}", fname, lname, prodid, prodqty)
                    .ToList();
                
                return orders;
            }
        }

        public IEnumerable<Orders> AddToOrder(int orderid, int prodid, int prodqty)
        {
            using (var context = new CornNuggetsContext())
            {

                var orders = context.Orders
                    .FromSqlRaw("EXECUTE dbo.spOrders_PlaceToStoreForCustomer {0}, {1}, {2}", orderid, prodid, prodqty)
                    .ToList();

                return orders;
            }
        }

        public IEnumerable<Orders> GetCustomerOrders(int customerid)
        {
            using (var context = new CornNuggetsContext())
            {
                var orders = context.Orders
                    .FromSqlInterpolated($"EXECUTE dbo.spCustomers_DisplayOdersByID({customerid})")
                    .ToList();
                return orders;
            }
        }
        public IEnumerable<OrderLog> GetOrdersDetails(int orderid)
        {
            using (var context = new CornNuggetsContext())
            {
                var orderlog = context.OrderLog
                    .FromSqlInterpolated($"EXECUTE dbo.spOrders_GetDetails, {orderid}")
                    .ToList();
                return orderlog;
            }
        }
        public IEnumerable<Customers> GetAllCustomers()
        {
            return _context.Customers;
        }
 
        
        
        
        public static void Run()
        {
            using (var context = new CornNuggetsContext())
            {
                #region FromSqlRaw
                var order = context.Orders
                    .FromSqlRaw("SELECT * FROM dbo.Orders")
                    .ToList();
                #endregion
            }

            using (var context = new CornNuggetsContext())
            {
                #region FromSqlRawStoredProcedure
                var cust = context.Customers
                    .FromSqlRaw("SELECT * FROM Customers ")
                    .ToList();
                #endregion
            }

            using (var context = new CornNuggetsContext())
            {
                #region FromSqlRawStoredProcedureParameter
                var fname = context.Customers.FromSqlRaw("Select FirstName from Customers where FirstName = firstName");
                var lname = context.Customers.FromSqlRaw("Select LastName from Customers where LastName = lastName");

                var cust = context.Customers
                    .FromSqlRaw("EXECUTE dbo.spCustomer_GetByFullName {0}, {1}", fname, lname)
                    .ToList();
                #endregion
            }

            using (var context = new CornNuggetsContext())
            {
                #region FromSqlInterpolatedStoredProcedureParameter
                var storeID = 1;

                var orders = context.Orders
                    .FromSqlInterpolated($"EXECUTE dbo.GetAllByStore {storeID}")
                    .ToList();
                #endregion
            }

            using (var context = new CornNuggetsContext())
            {
                #region FromSqlRawStoredProcedureSqlParameter
                var orders = new SqlParameter("OrderID", 1000);

                var blogs = context.Orders
                    .FromSqlRaw("EXECUTE dbo.GetDetails @orders", orders)
                    .ToList();
                #endregion
            }

            using (var context = new CornNuggetsContext())
            {
                #region FromSqlRawStoredProcedureNamedSqlParameter
                var product = new SqlParameter("ProductID", 111);
                var qty = new SqlParameter("ProductQty", 1);
                var fname = new SqlParameter("user", "john");
                var lname = new SqlParameter("user", "doe");
                
                var orders = context.Orders
                    .FromSqlRaw("EXECUTE dbo.spOrders_PlaceToStoreForCustomer @filterByOrderID=@OrderID", product)
                    .ToList();
                #endregion
            }

            using (var context = new CornNuggetsContext())
            {
                #region FromSqlInterpolatedComposed
                var searchTerm = 1;

                var orders = context.Orders
                    .FromSqlInterpolated($"SELECT * FROM dbo.spCustomers_DisplayOdersByID({searchTerm})")
                    .Where(b => b.OrderId == 3)
                    .OrderByDescending(b => b.OrderId)
                    .ToList();
                #endregion
            }

            using (var context = new CornNuggetsContext())
            {
                #region FromSqlInterpolatedAsNoTracking
                var searchTerm = "john";

                var blogs = context.Customers
                    .FromSqlInterpolated($"SELECT * FROM dbo.Search({searchTerm})")
                    .AsNoTracking()
                    .ToList();
                #endregion
            }

            using (var context = new CornNuggetsContext())
            {
                #region FromSqlInterpolatedInclude
                var searchTerm = "OrderID";

                var blogs = context.Orders
                    .FromSqlInterpolated($"SELECT * FROM dbo.Orders_AddToOrder({searchTerm})")
                    .Include(b => b.DateTimeStamp)
                    .ToList();
                #endregion
            }
        }
    }
}
