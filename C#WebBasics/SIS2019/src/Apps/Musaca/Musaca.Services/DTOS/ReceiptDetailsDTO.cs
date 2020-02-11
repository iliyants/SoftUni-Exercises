using System;
using System.Collections.Generic;
using System.Text;

namespace Musaca.Services.DTOS
{
    public class ReceiptDetailsDTO
    {
        public string Id { get; set; }

        public List<ProductView> Products { get; set; }
        public DateTime IssuedOn { get; set; }
        public string Cashier { get; set; }

        public string Total { get; set; }
    }
}
