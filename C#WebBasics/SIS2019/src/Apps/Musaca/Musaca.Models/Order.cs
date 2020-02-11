using Musaca.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Musaca.Models
{
    public class Order
    {
        public Order()
        {
            this.Id = Guid.NewGuid().ToString();
            this.OrderReceipts = new HashSet<ReceiptOrder>();
        }
        public string Id { get; set; }
        public OrderStatus Status { get; set; }
        public int Quantity { get; set; }
        public string CashierId { get; set; }
        public User Cashier { get; set; }
        public ICollection<ProductOrder> OrderProducts { get; set; }

        public ICollection<ReceiptOrder> OrderReceipts { get; set; }
    }
}
