using P03_SalesDatabase.Data;
using P03_SalesDatabase.Data.Models;
using System;
using System.Linq;

namespace P03_SalesDatabase
{
    public class Startup
    {
        static void Main(string[] args)
        {
            using (var context = new SalesContext())
            {
                context.Database.EnsureCreated();
                AddCustomer(context);
                context.SaveChanges();
                AddProduct(context);
                context.SaveChanges();
                AddStore(context);
                context.SaveChanges();
                AddSale(context);
                context.SaveChanges();

            };


        }

        private static void AddSale(SalesContext context)
        {
            var sale = new Sale();
            sale.CustomerId = context.Customers.Select(i => i.CustomerId).First();
            sale.ProductId = context.Products.Select(p => p.ProductId).First();
            sale.StoreId = context.Stores.Select(s => s.StoreId).First();

            context.Sales.Add(sale);

        }

        private static void AddStore(SalesContext context)
        {
            var store = new Store();
            store.Name = "OlEgofrenovi";

            context.Stores.Add(store);
        }

        private static void AddProduct(SalesContext context)
        {
            var product = new Product();
            product.Name = "Heski";
            product.Price = 12.5m;
            product.Quantity = 5;

            context.Products.Add(product);
        }

        private static void AddCustomer(SalesContext context)
        {
            var customer = new Customer();
            customer.CreditCardNumber = "12345678910";
            customer.Email = "someemail";
            customer.Name = "Pesho";

            context.Customers.Add(customer);
        }
    }
}
