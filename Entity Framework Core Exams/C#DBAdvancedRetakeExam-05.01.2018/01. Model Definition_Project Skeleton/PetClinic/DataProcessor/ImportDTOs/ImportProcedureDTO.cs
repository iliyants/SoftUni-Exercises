using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.ImportDTOs
{
    [XmlType("Procedure")]
    public class ImportProcedureDTO
    {
        [XmlElement("Vet")]
        [Required]
        [MinLength(3), MaxLength(40)]
        public string VetName { get; set; }

        [XmlElement("Animal")]
        [Required]
        [RegularExpression(@"[A-Za-z]{7}[\d]{3}")]
        public string AnimalSerialNumber { get; set; }

        [XmlElement("DateTime")]
        [Required]
        public string DateTime { get; set; }

        [XmlArray("AnimalAids")]
        [Required]
        public ImportAnimalAidNameDTO[] AnimalAids { get; set; }


    }
}
   //     <Vet>Niels Bohr</Vet>
   //     <Animal>acattee321</Animal>
	  //<DateTime>14-01-2016</DateTime>
   //     <AnimalAids>
