﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.Data.Models
{   
    public class Customer
    {
     
        [Key]
        public int Id { get; set; }

        [MinLength(3), MaxLength(20)]
        [Required]
        public string FirstName { get; set; }

        [MinLength(3), MaxLength(20)]
        [Required]
        public string LastName { get; set; }

        [Range(12,110)]
        [Required]
        public int Age { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]     
        [Required]
        public decimal Balance { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
            = new HashSet<Ticket>();
    }
}