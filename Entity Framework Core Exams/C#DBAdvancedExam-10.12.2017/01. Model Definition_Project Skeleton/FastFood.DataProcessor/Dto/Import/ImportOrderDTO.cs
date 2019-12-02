using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace FastFood.DataProcessor.Dto.Import
{
    [XmlType("Order")]
    public class ImportOrderDTO
    {
        [XmlElement("Customer")]
        [Required]
        public string CustomerName { get; set; }

        [XmlElement("Employee")]
        [Required]
        [MinLength(3), MaxLength(30)]
        public string EmployeeName { get; set; }

        [XmlElement("DateTime")]
        [Required]
        public string DateTime { get; set; }

        [XmlElement("Type")]
        [Required]
        public string OrderType { get; set; }

        [XmlArray("Items")]
        [Required]
        public ImportItemQuantityDTO[] Items { get; set; }

    }
}
