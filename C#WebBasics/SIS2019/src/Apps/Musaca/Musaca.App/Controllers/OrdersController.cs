using Musaca.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace Musaca.App.Controllers
{
    public class OrdersController:Controller
    {
        private readonly IOrderService orderService;
        private readonly IProductService productService;

        public OrdersController(IOrderService orderService, IProductService productService)
        {
            this.orderService = orderService;
            this.productService = productService;
        }

        [Authorize]
        public IActionResult Create(string barcode, int quantity)
        {
            var userId = this.User.Id;

            var product = this.productService.GetProductByBarcode(barcode);
            if (product == null)
            {
                return this.Redirect("/");
            }

            this.orderService.CreateOrder(product.Id, quantity, userId);

            return this.Redirect("/");
        }
    }
}
