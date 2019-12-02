using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ImportDTOs
{
    [XmlType("Purchase")]
   public class PurchaseDTO
    {
        [Required]
        [XmlAttribute("title")]
        public string GameTitle { get; set; }

        [Required]
        [XmlElement("Type")]
        public string PurchaseType { get; set; }

        [RegularExpression(@"[A-Z\d]{4}-[A-Z\d]{4}-[A-Z\d]{4}")]
        [Required]
        [XmlElement("Key")]
        public string ProductKey { get; set; }

        [RegularExpression(@"[\d]{4} [\d]{4} [\d]{4} [\d]{4}")]
        [Required]
        [XmlElement("Card")]
        public string CardNumber { get; set; }

        [Required]
        public string Date { get; set; }

    }
}
