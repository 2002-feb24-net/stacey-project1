using CornNuggets.DataAccess.Models;
using System.Linq;

namespace CornNuggets.DataAccess
{

        public static class DbInitializer
        {
            public static void Initialize(CornNuggetsContext context)
            {
                context.Database.EnsureCreated();

                // Look for any students.
                if (context.NuggetStores.Any())
                {
                    return;   // DB has been seeded
                }

                var stores = new NuggetStores[]
                {
                    new NuggetStores{StoreName="TEXA001",StoreLocation="Arlington"},
                    new NuggetStores{StoreName="TEXA002",StoreLocation="Dallas"},
                    new NuggetStores{StoreName="TEXA003",StoreLocation="Houston"},
                    new NuggetStores{StoreName="TEXA004",StoreLocation="Austin"},
                    new NuggetStores{StoreName="FLOR001",StoreLocation="Miami"},
                    new NuggetStores{StoreName="FLOR002",StoreLocation="Jacksonville"},
                    new NuggetStores{StoreName="FLOR003",StoreLocation="Daytona Beach"},
                    new NuggetStores{StoreName="FLOR004",StoreLocation="Hialeah"}
                };
                foreach (NuggetStores n in stores)
                {
                    context.NuggetStores.Add(n);
                }
                context.SaveChanges();

                var products = new Products[]
                {
                    new Products{ProductId = 111, ProductName= "Habenero",ProductPrice=4,Inventory=1000},
                    new Products{ProductId = 112, ProductName= "Nacho",ProductPrice=4,Inventory=1000},
                    new Products{ProductId = 113, ProductName= "Blue",ProductPrice=4,Inventory=1000},
                    new Products{ProductId = 222, ProductName= "Onion",ProductPrice=1,Inventory=1000},
                    new Products{ProductId = 223, ProductName= "Avocado",ProductPrice=1,Inventory=1000},
                    new Products{ProductId = 333, ProductName= "Fizzy",ProductPrice=2,Inventory=1000},
                    new Products{ProductId = 334, ProductName= "Tea",ProductPrice=2,Inventory=1000}
                };
                foreach (Products p in products)
                {
                    context.Products.Add(p);
                }
                context.SaveChanges();

                var customer = new Customers[]
                {
                    new Customers{FirstName="Stacey",LastName="Joseph",PreferredStore="TEXA001"},
                    new Customers{FirstName="Euri",LastName="Joseph",PreferredStore="TEXA002"},
                    new Customers{FirstName="Eunique",LastName="Joseph",PreferredStore="TEXA002"},
                    new Customers{FirstName="Herbby",LastName="Joseph",PreferredStore="FLOR001"},
                    new Customers{FirstName="Euriah",LastName="Joseph",PreferredStore="TEXA001"},
                    new Customers{FirstName="Gary",LastName="Joseph",PreferredStore="NYOR001"},
                    new Customers{FirstName="Derrell",LastName="Brown",PreferredStore="SCAR001"},
                    
                };
                foreach (Customers c in customer)
                {
                    context.Customers.Add(c);
                }
                context.SaveChanges();
      
            }
        }
    
}
