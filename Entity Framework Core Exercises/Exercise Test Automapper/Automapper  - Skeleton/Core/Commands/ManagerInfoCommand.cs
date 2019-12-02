using AutoMapper;
using Core.Commands.Contracts;
using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModels;

namespace Core.Commands
{
    public class ManagerInfoCommand : ICommand
    {

        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public ManagerInfoCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public string Execute(string[] inputArguments)
        {
            var managerId = int.Parse(inputArguments[0]);


            var manager = this.context.Employees
                .Include(x => x.ManagedEmployees)
                .FirstOrDefault(x => x.Id == managerId);

            if (manager == null)
            {
                throw new ArgumentNullException("There is no employee with the given Id in the database");
            }

            var managerDto = this.mapper.CreateMappedObject<ManagerDto>(manager);

            var sb = new StringBuilder();

            sb.AppendLine($"{managerDto.FirstName} {managerDto.LastName} | " +
                $"Employees: {managerDto.ManagedEmployees.Count}");

            foreach (var employee in managerDto.ManagedEmployees)
            {
                sb.AppendLine($"- {employee.FirstName} {employee.LastName} - ${employee.Salary:F2}");
            }

            return sb.ToString().Trim();


        }
    }
}
