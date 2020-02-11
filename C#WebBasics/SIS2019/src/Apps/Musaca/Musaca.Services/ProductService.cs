using Musaca.Data;
using Musaca.Models;
using System;
using System.Linq;
namespace Musaca.Services
{
    public class ProductService : IProductService
    {

        private readonly MusacaDbContext context;

        public ProductService(MusacaDbContext context)
        {
            this.context = context;
        }
        public bool CheckForExistingProduct(string name)
        {
            var productNames = this.context.Products.Select(x => x.Name).ToList();

            return !productNames.Contains(name);
        }

        public void CreateProduct(string name, decimal price, string barcode, string picture)
        {
            var product = new Product()
            {
                Name = name,
                Price = price,
                Barcode = barcode,
                Picture = picture
            };

            this.context.Products.Add(product);
            this.context.SaveChanges();
        }

        public void DeleteProduct(string productId)
        {
            var product = this.context.Products.SingleOrDefault(x => x.Id == productId);
            this.context.Products.Remove(product);
            this.context.SaveChanges();
        }


        public IQueryable<Product> GetAllProductsQuery()
        {
            return this.context.Products;
        }

        public Product GetProductByBarcode(string barcode)
        {
            return this.context.Products.SingleOrDefault(x => x.Barcode == barcode);
        }
    }
}
