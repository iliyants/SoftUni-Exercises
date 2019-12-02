using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.DataProcessor.ImportDTOs
{
    public class CardDTO
    {
        [RegularExpression(@"[\d]{4} [\d]{4} [\d]{4} [\d]{4}")]
        [Required]
        public string Number { get; set; }

        [RegularExpression(@"[\d]{3}")]
        [Required]
        public string Cvc { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
