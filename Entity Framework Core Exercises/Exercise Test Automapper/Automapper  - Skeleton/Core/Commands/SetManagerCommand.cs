using AutoMapper;
using Core.Commands.Contracts;
using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Commands
{
    public class SetManagerCommand : ICommand
    {
        private readonly MyAppContext context;

        public SetManagerCommand(MyAppContext context)
        {
            this.context = context;
        }

        public string Execute(string[] inputArguments)
        {
            int managerId = int.Parse(inputArguments[0]);
            int employeeId = int.Parse(inputArguments[1]);

            var employee = this.context.Employees.Find(employeeId);
            var manager = this.context.Employees.Find(managerId);

            if(employee == null || manager == null)
            {
                throw new NullReferenceException("Invalid manager Id or employee Id");
            }

            employee.Manager = manager;
            this.context.SaveChanges();

            return
                $"Employee {employee.FirstName} {employee.LastName}" +
                $" is now being managed by {manager.FirstName} {manager.LastName}";

        }
    }
}
