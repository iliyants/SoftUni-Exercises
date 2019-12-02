using AutoMapper;
using Core.Commands.Contracts;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModels;

namespace Core.Commands
{
    public class ListEmployeesOlderThanCommand : ICommand
    {

        private readonly MyAppContext context;

        public ListEmployeesOlderThanCommand(MyAppContext context)
        {
            this.context = context;
        }

        public string Execute(string[] inputArguments)
        {

            var age = int.Parse(inputArguments[0]);

            var employees = this.context.Employees
                .Where(x => DateTime.Now.Year - x.Birthday.Year > age)
                .Select(x => new
                {
                    EmployeeFullName = $"{x.FirstName} {x.LastName}",
                    x.Salary,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName
                })
                .OrderByDescending(x => x.Salary)
                .ToList();


            return string.Join(Environment.NewLine, employees
                .Select(x => $"{x.EmployeeFullName} - {x.Salary:F2} - Manager: " +
                 (string.IsNullOrEmpty(x.ManagerFirstName) ? "[no manager]" :
                 $"{x.ManagerFirstName} {x.ManagerLastName}")));

        }
    }
}
