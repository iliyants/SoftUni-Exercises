using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.Data.Models
{
    public class OfficerPrisoner
    {
        [Key]
        public int PrisonerId { get; set; }

        public Prisoner Prisoner { get; set; }

        [Key]
        public int OfficerId { get; set; }
        public Officer Officer { get; set; }
    }
}
