using Models;
using System.Collections.Generic;

namespace ViewModels
{
    public class ManagerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }
        public List<EmployeeDto> ManagedEmployees { get; set; }
         = new List<EmployeeDto>();
    }
}
