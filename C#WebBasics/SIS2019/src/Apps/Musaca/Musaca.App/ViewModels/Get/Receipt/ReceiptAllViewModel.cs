using System;
using System.Collections.Generic;
using System.Text;

namespace Musaca.App.ViewModels.Get.Receipt
{
    public class ReceiptAllViewModel
    {
        public string Id { get; set; }

        public DateTime IssuedOn { get; set; }
        public string Cashier { get; set; }

        public decimal Total { get; set; }
    }
}
