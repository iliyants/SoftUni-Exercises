using Core.Commands.Contracts;
using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Commands
{
    public class EmployeePersonalInfoCommand:ICommand
    {
        private readonly MyAppContext context;
        public EmployeePersonalInfoCommand(MyAppContext context)
        {
            this.context = context;
        }
        public string Execute(string[] inputArguments)
        {
            var employeeId = int.Parse(inputArguments[0]);

            var employee = context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentNullException($"No employee with id" +
                    $" - {employeeId} was found in the database");
            }

            
            return $"ID: {employee.Id} - {employee.FirstName} {employee.LastName} - " +
                $"${employee.Salary:F2}\n" +
                $"Birthday: {employee.Birthday.Date.ToShortDateString()}\n" +
                $"Address: {employee.Address}\n";

        }
    }
}
