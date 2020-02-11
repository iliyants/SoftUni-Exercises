using System;
using System.Collections.Generic;
using System.Text;

namespace Musaca.Models
{
    public class ReceiptOrder
    {

        public string OrderId { get; set; }
        public Order Order { get; set; }

        public string ReceiptId { get; set; }
        public Receipt Receipt { get; set; }
    }
}
