using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Models
{
    public class Employee
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Range(typeof(decimal),"0.00","1000000000")]
        public decimal Salary { get; set; }

        public DateTime Birthday { get; set; }
        public string Address { get; set; }

        public int? ManagerId { get; set; }
        public Employee Manager { get; set; }

        public List<Employee> ManagedEmployees { get; set; }
         = new List<Employee>();

    }
}
