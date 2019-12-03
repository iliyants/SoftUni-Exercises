using System;
using System.Collections.Generic;
using System.Text;

namespace PetClinic.Models
{
    public class Vet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Profession { get; set; }

        public int Age { get; set; }

        public string PhoneNumber { get; set; }

        public ICollection<Procedure> Procedures { get; set; }
         = new HashSet<Procedure>();
    }
}

