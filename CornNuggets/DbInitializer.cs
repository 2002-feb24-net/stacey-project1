using CornNuggets.DataAccess;
using CornNuggets.DataAccess.Models;
using System.Linq;

namespace CornNuggets
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
                    new Products{ProductName= "Habenero",ProductPrice=4,Inventory=1000},
                    new Products{ProductName= "Nacho",ProductPrice=4,Inventory=1000},
                    new Products{ProductName= "Blue",ProductPrice=4,Inventory=1000},
                    new Products{ProductName= "Onion",ProductPrice=1,Inventory=1000},
                    new Products{ProductName= "Avocado",ProductPrice=1,Inventory=1000},
                    new Products{ProductName="Fizzy",ProductPrice=2,Inventory=1000},
                    new Products{ProductName="Tea",ProductPrice=2,Inventory=1000}
                };
                foreach (Products c in products)
                {
                    context.Products.Add(c);
                }
                context.SaveChanges();

                var customer = new Customers[]
                {
            new Customers{FirstName="Stacey",LastName="Joseph",PreferredStore="TEXA001"},
            new Customers{FirstName="Stacey",LastName="Joseph",PreferredStore="TEXA001"},
            new Customers{FirstName="Stacey",LastName="Joseph",PreferredStore="TEXA001"},
            new Customers{FirstName="Stacey",LastName="Joseph",PreferredStore="TEXA001"},
            new Customers{FirstName="Stacey",LastName="Joseph",PreferredStore="TEXA001"},
            new Customers{FirstName="Stacey",LastName="Joseph",PreferredStore="TEXA001"},
            new Customers{FirstName="Stacey",LastName="Joseph",PreferredStore="TEXA001"},
            new Customers{FirstName="Stacey",LastName="Joseph",PreferredStore="TEXA001"},
            new Customers{FirstName="Stacey",LastName="Joseph",PreferredStore="TEXA001"},
            new Customers{FirstName="Stacey",LastName="Joseph",PreferredStore="TEXA001"},
            new Customers{FirstName="Stacey",LastName ="Joseph",PreferredStore="TEXA001"},
            new Customers{FirstName="Stacey",LastName="Joseph",PreferredStore="TEXA001"},
                };
                foreach (Customers c in customer)
                {
                    context.Customers.Add(c);
                }
                context.SaveChanges();
            }
        }
    
}
