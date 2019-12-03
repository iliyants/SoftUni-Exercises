using System;
using System.Collections.Generic;
using System.Text;

namespace PetClinic.Models
{
    public class AnimalAid
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public ICollection<ProcedureAnimalAid> AnimalAidProcedures { get; set; }
         = new HashSet<ProcedureAnimalAid>();
    }
}


