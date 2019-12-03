using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetClinic.Models
{
    public class Procedure
    {
        public int Id { get; set; }

        public int AnimalId { get; set; }
        public Animal Animal { get; set; }

        public int VetId { get; set; }
        public Vet Vet { get; set; }

        public ICollection<ProcedureAnimalAid> ProcedureAnimalAids { get; set; }
         = new HashSet<ProcedureAnimalAid>();

        public decimal Cost => this.ProcedureAnimalAids.Sum(x => x.Procedure.Cost);

        public DateTime DateTime { get; set; }
    }
}



