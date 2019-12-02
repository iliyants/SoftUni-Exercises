using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.DTOs
{
    public class UsersSoldProductsDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public List<ProductDTO> ProductsSold { get; set; }
         = new List<ProductDTO>();

    }
}
