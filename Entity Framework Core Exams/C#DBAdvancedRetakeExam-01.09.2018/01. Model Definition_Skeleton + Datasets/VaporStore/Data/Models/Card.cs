using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VaporStore.Data.Enums;

namespace VaporStore.Data.Models
{
    public class Card
    {
        [Key]
        public int Id { get; set; }

        [RegularExpression(@"[\d]{4} [\d]{4} [\d]{4} [\d]{4}")]
        [Required]
        public string Number { get; set; }

        [RegularExpression(@"[\d]{3}")]
        [Required]
        public string Cvc { get; set; }

        [Required]
        public CardType Type { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public User User { get; set; }

        public ICollection<Purchase> Purchases { get; set; }
         = new HashSet<Purchase>();



    }
}
