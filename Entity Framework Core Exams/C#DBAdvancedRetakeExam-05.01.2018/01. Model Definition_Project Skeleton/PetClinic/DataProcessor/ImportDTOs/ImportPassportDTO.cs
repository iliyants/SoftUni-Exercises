using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetClinic.DataProcessor.ImportDTOs
{
    public class ImportPassportDTO
    {
        [Required]
        [RegularExpression(@"[A-Za-z]{7}[\d]{3}")]
        public string SerialNumber { get; set; }

        [Required]
        [MinLength(3),MaxLength(30)]
        public string OwnerName { get; set; }

        [Required]
        [RegularExpression(@"\+359[\d]{9}|0[\d]{9}")]
        public string OwnerPhoneNumber { get; set; }

        [Required]
        public string RegistrationDate { get; set; }
    }
}
      //"SerialNumber": "etyhGgH678",
      //"OwnerName": "Sheldon Cooper",
      //"OwnerPhoneNumber": "0897556446",
      //"RegistrationDate": "12-03-2014"
