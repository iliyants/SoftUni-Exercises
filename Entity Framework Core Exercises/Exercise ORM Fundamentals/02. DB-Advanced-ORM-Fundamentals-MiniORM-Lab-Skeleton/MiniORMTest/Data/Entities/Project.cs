using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiniORMTest.Data.Entities
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<EmployeeProject> EmployeePrjects { get; }
    }
}
