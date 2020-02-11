using Musaca.App.ViewModels.Get.Product;
using Musaca.App.ViewModels.Get.Receipt;
using Musaca.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using System.Linq;

namespace Musaca.App.Controllers
{
    public class ReceiptsController:Controller
    {

        private readonly IReceiptService receiptService;
        private readonly IOrderService orderService;

        public ReceiptsController(IReceiptService receiptService, IOrderService orderService)
        {
            this.receiptService = receiptService;
            this.orderService = orderService;
        }

        [Authorize]
        public IActionResult All()
        {

            var allReceipts = this.receiptService.GetAllReceipts();

            var model = allReceipts
              .Where(x => x.ReceiptOrders.All(order => order.Order.Status.ToString() == "Completed"))
              .Select(x => new ReceiptAllViewModel()
              {
                  Id = x.Id,
                  Cashier = x.Cashier.Username,
                  IssuedOn = x.IssuedOn,
                  Total = x.ReceiptOrders
                  .Select(s => s.Order.Quantity * s.Order.OrderProducts.Sum(p => p.Product.Price)).Sum()
              }).ToList();

            return this.View(model);
        }

        [Authorize]
        public IActionResult Details(string id)
        {

            var model = this.receiptService.GetReceiptDetails(id);
            
            return this.View(model);
        }

        [HttpPost]
        public IActionResult Create()
        {
            var userId = this.User.Id;
            var activeOrders = this.orderService.GetAllOrdersWithStatusActive(userId);
            this.receiptService.CreateReceipt(userId, activeOrders);
            this.orderService.ChangeOrdersStatusToCompleted(activeOrders);

            return this.Redirect("/Users/Profile");
        }
    }
}
