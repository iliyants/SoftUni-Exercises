﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicHub.Data.Models
{
    public class Producer
    {
        [Key]
        public int Id { get; set; }

        [MinLength(3), MaxLength(30)]
        [Required]
        public string Name { get; set; }

        [RegularExpression("[A-Z][a-z]+ [A-Z][a-z]+")]
        public string Pseudonym { get; set; }

        [RegularExpression("[+359]+ [0-9]{3} [0-9]{3} [0-9]{3}")]
        public string PhoneNumber { get; set; }

        public ICollection<Album> Albums { get; set; }
         = new HashSet<Album>();
    }
}
