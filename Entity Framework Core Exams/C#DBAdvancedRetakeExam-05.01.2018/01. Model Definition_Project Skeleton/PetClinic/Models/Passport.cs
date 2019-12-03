using System;
using System.Collections.Generic;
using System.Text;

namespace PetClinic.Models
{
    public class Passport
    {
        //PRIMARY KEY
        public string SerialNumber { get; set; }

        public Animal Animal { get; set; }

        public string OwnerPhoneNumber { get; set; }

        public string OwnerName { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}


