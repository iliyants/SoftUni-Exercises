using AutoMapper;
using Core.Commands.Contracts;
using Data;
using System;


namespace Core.Commands
{
    public class SetBirthdayCommand : ICommand
    {

        private readonly MyAppContext context;
        public SetBirthdayCommand(MyAppContext context)
        {
            this.context = context;
        }
        public string Execute(string[] inputArguments)
        {
            var employeeId = int.Parse(inputArguments[0]);

            var date = DateTime.ParseExact(inputArguments[1], "dd-MM-yyyy",
            System.Globalization.CultureInfo.InvariantCulture);

            var employee = context.Employees.Find(employeeId);

            if(employee == null)
            {
                throw new ArgumentNullException($"No employee with id" +
                    $" - {employeeId} was found in the database");
            }

            employee.Birthday = date;
            context.SaveChanges();

            return $"Employee {employee.FirstName}" +
                $" {employee.LastName}`s birthday was set to {date.ToString()}";

        }
    }
}
