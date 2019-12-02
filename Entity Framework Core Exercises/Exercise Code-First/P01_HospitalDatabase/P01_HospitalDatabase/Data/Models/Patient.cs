namespace P01_HospitalDatabase.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Required]
        public string Address { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string Email { get; set; }

        [Required]
        public bool HasInsurance { get; set; }

        public ICollection<Visitation> Visitations { get; set; }
         = new HashSet<Visitation>();

        public ICollection<Diagnose> Diagnoses { get; set; }
        = new HashSet<Diagnose>();

        public ICollection<PatientMedicament> Prescriptions { get; set; }
        = new HashSet<PatientMedicament>();
    }
}
