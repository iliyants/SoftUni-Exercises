using Musaca.App.ViewModels.Get.Product;
using System;
using System.Collections.Generic;

namespace Musaca.App.ViewModels.Get.Receipt
{
    public class ReceiptDetailsView
    {
        public string Id { get; set; }

        public List<ProductOrderView> OrderProducts { get; set; }
        public DateTime IssuedOn { get; set; }
        public string Cashier { get; set; }

        public decimal Total { get; set; }
    }
}
