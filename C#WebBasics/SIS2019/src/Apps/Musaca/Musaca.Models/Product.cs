using System;
using System.Collections.Generic;

namespace Musaca.Models
{
    public class Product
    {
        public Product()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ProductOrders = new HashSet<ProductOrder>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Barcode { get; set; }
        public string Picture { get; set; }

        public ICollection<ProductOrder> ProductOrders { get; set; }

    }
}
