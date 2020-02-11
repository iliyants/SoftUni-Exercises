using Musaca.Models;
using System.Linq;

namespace Musaca.Services
{
    public interface IProductService
    {
        void CreateProduct(string name, decimal price, string barcode, string picture);

        bool CheckForExistingProduct(string name);

        IQueryable<Product> GetAllProductsQuery();

        void DeleteProduct(string productId);

        Product GetProductByBarcode(string barcode);

    }
}
