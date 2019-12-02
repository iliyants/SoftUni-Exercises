﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniORMTest.Data.Entities
{
    public class EmployeeProject
    {
        [Key]
        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }
        [Key]
        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }

        public Project Project { get; set; }

        public Employee Employee { get; set; }
    }
}
