using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftUni
{
   public class StartUp
    {
        static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                Console.WriteLine(RemoveTown(context));
            }
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                            .Select(x => new
                            {
                                x.EmployeeId,
                                x.FirstName,
                                x.LastName,
                                x.MiddleName,
                                x.JobTitle,
                                x.Salary
                            })
                            .OrderBy(x => x.EmployeeId);

            var sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}");
            }

            return sb.ToString().Trim();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                            .Select(x => new
                            {
                                x.FirstName,
                                x.Salary
                            })
                            .Where(x => x.Salary > 50000)
                            .OrderBy(x => x.FirstName);

            var sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:F2}");
            }

            return sb.ToString().Trim();

        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                            .Select(x => new
                            {
                                x.FirstName,
                                x.LastName,
                                DepartmentName = x.Department.Name,
                                x.Salary
                            })
                            .Where(x => x.DepartmentName == "Research and Development")
                            .OrderBy(x => x.Salary)
                            .ThenByDescending(x => x.FirstName);

            var sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:F2}");
            }

            return sb.ToString().Trim();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var adress = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var nakov = context.Employees
                    .FirstOrDefault(e => e.LastName == "Nakov");

            nakov.Address = adress;

            context.SaveChanges();

            var sb = new StringBuilder();

            var employees = context.Employees
                            .Select(x => new
                            {
                                x.AddressId,
                                AdressText = x.Address.AddressText
                            })
                            .OrderByDescending(x => x.AddressId)
                            .Take(10)
                            .ToList();


            foreach (var e in employees)
            {
                sb.AppendLine($"{e.AdressText}");
            }          

            return sb.ToString().Trim();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                             .Where(x => x.EmployeesProjects.Any(y => y.Project.StartDate.Year >= 2001 &&
                             y.Project.StartDate.Year <= 2003))
                             .Select(x => new
                             {
                                 EmployeeFullName = x.FirstName + ' ' + x.LastName,
                                 ManagerFullName = x.Manager.FirstName + ' ' + x.Manager.LastName,
                                 Projects = x.EmployeesProjects.Select(p => new
                                 {
                                     ProjectName = p.Project.Name,
                                     StartDate = p.Project.StartDate,
                                     EndDate = p.Project.EndDate
                                 }).ToList()
                             })
                             .Take(10)
                             .ToList();
            var sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.EmployeeFullName} - Manager: {e.ManagerFullName}");

                foreach (var p in e.Projects)
                {
                    var startDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt");
                    string endDate = p.EndDate != null
                        ? p.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt")
                        : "not finished";

                    sb.AppendLine($"--{p.ProjectName} - {startDate} - {endDate}");
                };
            }

            return sb.ToString().Trim();
          
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                            .Select(x => new
                            {
                                x.AddressText,
                                Count = x.Employees.Count,
                                TownName = x.Town.Name
                            })
                            .OrderByDescending(x => x.Count)
                            .ThenBy(x => x.TownName)
                            .ThenBy(x => x.AddressText)
                            .Take(10)
                            .ToList();



            var sb = new StringBuilder();

            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.AddressText}, {a.TownName}" +
                    $" - {a.Count} employees");
            }

            return sb.ToString().Trim();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
                            .Where(x => x.EmployeeId == 147)
                           .Select(x => new
                           {
                               FirstName = x.FirstName,
                               LastName = x.LastName,
                               JobTitle = x.JobTitle,
                               Projects = x.EmployeesProjects.Select(p => new
                               {
                                   ProjectName = p.Project.Name
                               })
                               .OrderBy(p => p.ProjectName)
                               .ToList()
                           })
                           .First();
                           


            var sb = new StringBuilder();

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (var p in employee.Projects)
            {
               sb.AppendLine($"{p.ProjectName}");           
            }

            return sb.ToString().Trim();
                           
                            
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                                .Where(x => x.Employees.Count > 5)
                                .Select(x => new
                                {
                                    DepartmentName = x.Name,
                                    ManagerFirstName = x.Manager.FirstName,
                                    ManagerLastName = x.Manager.LastName,
                                    Employees = x.Employees.Select(e => new
                                    {
                                        EmployeeFirstName = e.FirstName,
                                        EmployeeLastName = e.LastName,
                                        EmployeeJob = e.JobTitle
                                    }).OrderBy(e => e.EmployeeFirstName)
                                        .ThenBy(e => e.EmployeeLastName)
                                        .ToList()
                                })
                                .ToList()
                                .OrderBy(x => x.Employees.Count)
                                .ThenBy(x => x.DepartmentName);

            var sb = new StringBuilder();

            foreach (var dep in departments)
            {
                sb.AppendLine($"{dep.DepartmentName} - {dep.ManagerFirstName} {dep.ManagerLastName}");

                foreach (var emp in dep.Employees)
                {
                    sb.AppendLine($"{emp.EmployeeFirstName} {emp.EmployeeLastName} - {emp.EmployeeJob}");
                }
            }

            return sb.ToString().Trim();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                            .Select(x => new
                            {
                                Name = x.Name,
                                Description = x.Description,
                                StartDate = x.StartDate
                            })
                            .OrderByDescending(x => x.StartDate)
                            .Take(10)
                            .ToList();

            var sb = new StringBuilder();

            foreach (var p in projects.OrderBy(x => x.Name))
            {
                sb.AppendLine($"{p.Name}");
                sb.AppendLine($"{p.Description}");
                sb.AppendLine($"{p.StartDate}");

            }

            return sb.ToString().Trim();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {

            var employees = context.Employees
                            .Where(x => x.Department.Name == "Engineering" ||
                                x.Department.Name == "Tool Design" ||
                                x.Department.Name == "Marketing" ||
                                x.Department.Name == "Information Services")
                            .Select(x => new
                            {
                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                Salary = x.Salary + (x.Salary * 0.12m)
                            })
                            .OrderBy(x => x.FirstName)
                            .ThenBy(x => x.LastName)
                            .ToList();

            var sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:F2})");
            }

            return sb.ToString().Trim();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                            .Where(x => x.FirstName.StartsWith("Sa"))
                            .Select(x => new
                            {
                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                JobTitle = x.JobTitle,
                                Salary = x.Salary
                            })
                            .OrderBy(x => x.FirstName)
                            .ThenBy(x => x.LastName)
                            .ToList();
            var sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})");
            }

            return sb.ToString().Trim();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var employeesProjects = context.EmployeesProjects
                                    .Where(x => x.ProjectId == 2)
                                    .ToList();

            foreach (var p in employeesProjects)
            {
                context.EmployeesProjects.Remove(p);
            }

            var project = context.Projects.FirstOrDefault(x => x.ProjectId == 2);

            context.Projects.Remove(project);

            context.SaveChanges();

            var projects = context.Projects
                            .Select(x => new
                            {
                                Name = x.Name
                            })
                            .ToList();

            var sb = new StringBuilder();

            foreach (var p in projects)
            {
                sb.AppendLine($"{p.Name}");
            }

            return sb.ToString().Trim();
                                    
        }

        public static string RemoveTown(SoftUniContext context)
        {
            int townId = context.Towns
                        .Where(x => x.Name == "Seattle")
                        .Select(x => x.TownId)
                        .FirstOrDefault();


            var employees = context.Employees
                            .Where(x => x.Address.TownId == townId)
                            .ToList();


            foreach (var e in employees)
            {
                e.AddressId = null;
            }

            var addresses = context.Addresses
                  .Where(x => x.TownId == townId)
                  .ToList();

            foreach (var a in addresses)
            {
                context.Addresses.Remove(a);
            }

            var town = context.Towns
            .Where(x => x.TownId == townId)
            .FirstOrDefault();

            context.Towns.Remove(town);
            context.SaveChanges();

            return $"{addresses.Count} addresses in Seattle were deleted";
        }


    }
}
