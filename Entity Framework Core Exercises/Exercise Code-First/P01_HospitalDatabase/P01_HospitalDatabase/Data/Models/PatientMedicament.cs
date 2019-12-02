namespace P01_HospitalDatabase.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PatientMedicament
    {
        [Key]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [Key]
        public int MedicamentId { get; set; }
        public Medicament Medicament { get; set; }

    }
}
