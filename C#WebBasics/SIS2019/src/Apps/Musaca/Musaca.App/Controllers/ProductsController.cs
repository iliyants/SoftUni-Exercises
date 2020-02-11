using Musaca.App.ViewModels.Get.Product;
using Musaca.App.ViewModels.Post.Product;
using Musaca.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using System.Linq;
using System.Net;

namespace Musaca.App.Controllers
{
    public class ProductsController : Controller
    {

        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(ProductCreateView model)
        {
            var productNameIsUnique = this.productService.CheckForExistingProduct(model.Name);
            var validModelState = ModelState.IsValid;

            if (!validModelState|| !productNameIsUnique)
            {
                return this.Create();
            }

            this.productService.CreateProduct(model.Name, model.Price, model.Barcode, model.Picture);

            return this.All();
        }

        [Authorize]
        public IActionResult All()
        {

            var model = this.productService.GetAllProductsQuery().Select(x => new ProductAllView()
            {
                Id = x.Id,
                Name = x.Name,
                Picture = WebUtility.UrlDecode(x.Picture),
                Barcode = x.Barcode,
                Price = x.Price
            }).ToList();

            return this.View(model);
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
            this.productService.DeleteProduct(id);

            return this.All();
        }
    }
}
