using Core.Commands.Contracts;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Commands
{
    public class SetAddressCommand:ICommand
    {
        private readonly MyAppContext context;
        public SetAddressCommand(MyAppContext context)
        {
            this.context = context;
        }
        public string Execute(string[] inputArguments)
        {
            var employeeId = int.Parse(inputArguments[0]);

            var address = string.Join(" ", inputArguments.Skip(1));

            var employee = context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentNullException($"No employee with id" +
                    $" - {employeeId} was found in the database");
            }

            employee.Address = address;
            context.SaveChanges();

            return $"Employee {employee.FirstName}" +
                $" {employee.LastName}`s address was set to {employee.Address}";

        }
    }
}
