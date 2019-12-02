using System;
using System.ComponentModel.DataAnnotations;

namespace CarDealer.DTO
{
    public class CustomerInfoDTO
    {
        public string Name { get; set; }

        public string BirthDate { get; set; }

        public bool IsYoungDriver { get; set; }
    }
}
