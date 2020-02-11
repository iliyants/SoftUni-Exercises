using Musaca.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Musaca.Models
{
    public class User
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Receipts = new HashSet<Receipt>();
            this.Orders = new HashSet<Order>();
        }
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public UserRole Role { get; set; }

        public ICollection<Receipt> Receipts { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
