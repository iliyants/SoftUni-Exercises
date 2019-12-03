using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.ImportDTOs
{
    [XmlType("Vet")]
    public class ImportVetDTO
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(3), MaxLength(40)]
        public string Name { get; set; }

        [XmlElement("Profession")]
        [Required]
        [MinLength(3),MaxLength(50)]
        public string Profession { get; set; }

        [XmlElement("Age")]
        [Required]
        [Range(22,65)]
        public int Age { get; set; }

        [XmlElement("PhoneNumber")]
        [Required]
        [RegularExpression(@"\+359[\d]{9}|0[\d]{9}")]
        public string PhoneNumber { get; set; }
    }
}
