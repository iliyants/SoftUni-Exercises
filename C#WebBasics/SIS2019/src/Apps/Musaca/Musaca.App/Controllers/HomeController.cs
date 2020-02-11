using Musaca.App.ViewModels.Get.Product;
using Musaca.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Result;
using System.Linq;

namespace Musaca.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrderService orderService;

        public HomeController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet(Url = "/")]
        public IActionResult Slash()
        {
            return this.Index();
        }

        public IActionResult Index()
        {
            if (this.IsLoggedIn())
            {
                return this.IndexLogged();
            }
            return this.View();
        }

        public IActionResult IndexLogged()
        {

            var userId = this.User.Id;

            var model = this.orderService
                .GetOrderProductsByUser(userId)
                .Where(x => x.Status.ToString() == "Active")
                .Select(x => new ProductOrderView()
            {
                Name = x.OrderProducts.Select(product => product.Product.Name).SingleOrDefault(),
                Quantity = x.Quantity,
                Price = x.OrderProducts.Select(product => product.Product.Price).SingleOrDefault()
            }).ToList();


            return this.View(model);
        }

    }
}
