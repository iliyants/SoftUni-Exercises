using AutoMapper;
using Core.Commands.Contracts;
using Data;
using Models;
using ViewModels;

namespace Core.Commands
{
    public class AddEmployeeCommand : ICommand
    {

        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public AddEmployeeCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] inputArguments)
        {
      
            var firstName = inputArguments[0];
            var lastName = inputArguments[1];
            var salary = decimal.Parse(inputArguments[2]);

            var employee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Salary = salary
            };

            this.context.Employees.Add(employee);
            this.context.SaveChanges();

            var employeeDto = this.mapper.CreateMappedObject<EmployeeDto>(employee);

            var result =
               $"Employee: {employeeDto.FirstName} {employeeDto.LastName}, with a salary of " +
               $"{employeeDto.Salary} was registered succsessfully";

            return result;

        }
    }
}
