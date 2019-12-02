using FastFood.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FastFood.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string Customer { get; set; }

        public DateTime DateTime { get; set; }

        public OrderType Type { get; set; }

        public decimal TotalPrice
         => this.OrderItems.Sum(oi => oi.Quantity * oi.Item.Price);
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
         = new HashSet<OrderItem>();
    }
}
