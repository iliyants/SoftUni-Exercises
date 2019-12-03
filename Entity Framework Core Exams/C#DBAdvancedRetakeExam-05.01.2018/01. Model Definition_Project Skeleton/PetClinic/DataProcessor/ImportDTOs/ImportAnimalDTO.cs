using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetClinic.DataProcessor.ImportDTOs
{
    class ImportAnimalDTO
    {
        [Required]
        [MinLength(3), MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        public string Type { get; set; }

        [Required]
        [Range(1, 100)]
        public int Age { get; set; }

        [Required]
        public ImportPassportDTO Passport { get; set; }
    }
}
//"Name":"Bella",
//    "Type":"cat",
//    "Age": 2,
//    "Passport": 
