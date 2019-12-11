namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using System.Xml.Serialization;
    using System.IO;
    using TeisterMask.DataProcessor.ImportDto;
    using System.Text;
    using System.Linq;
    using TeisterMask.Data.Models;
    using System.Globalization;
    using TeisterMask.Data.Models.Enums;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        private const string dFormat = "dd/MM/yyyy";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var projectsDTO = DeserializedCollection<ImportProjectDTO>("Projects", xmlString);

            var sb = new StringBuilder();


            foreach (var projectDTO in projectsDTO)
            {
                if (!IsValid(projectDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }   

                var project = new Project()
                {
                    Name = projectDTO.Name,
                    OpenDate = ConvertToDate(projectDTO.OpenDate),
                    DueDate = string.IsNullOrEmpty(projectDTO.DueDate) ?
                     null : (DateTime?)ConvertToDate(projectDTO.DueDate)
                };

                foreach (var taskDTO in projectDTO.Tasks)
                {

                    if (!IsValid(taskDTO))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var task = new Task()
                    {
                        Name = taskDTO.Name,
                        OpenDate = ConvertToDate(taskDTO.OpenDate),
                        DueDate = ConvertToDate(taskDTO.DueDate),
                        ExecutionType = (ExecutionType)taskDTO.ExecutionType,
                        LabelType = (LabelType)taskDTO.LabelType
                    };

                    if (project.DueDate != null)
                    {
                        if(task.DueDate > project.DueDate)
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }
                    }
                    if (task.OpenDate < project.OpenDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    project.Tasks.Add(task);
                }

                context.Projects.Add(project);
                sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count()));

            }
            context.SaveChanges();
            return sb.ToString();

        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var employeesDTO = JsonConvert.DeserializeObject<ImportEmployeeDTO[]>(jsonString);

            var sb = new StringBuilder();

            foreach (var employeeDTO in employeesDTO)
            {

                if (!IsValid(employeeDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var employee = new Employee()
                {
                    Username = employeeDTO.Username,
                    Email = employeeDTO.Email,
                    Phone = employeeDTO.Phone
                };


                foreach (var taskId in employeeDTO.Tasks.Distinct())
                {
                    var task = context.Tasks.FirstOrDefault(x => x.Id == taskId);

                    if (task == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var employeeTask = new EmployeeTask()
                    {
                        Employee = employee,
                        Task = task
                    };


                    employee.EmployeesTasks.Add(employeeTask);
                }
                context.Employees.Add(employee);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));

                context.SaveChanges();
            }

            return sb.ToString();


        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        public static T[] DeserializedCollection<T>(string rootAttribute, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(T[]),
                       new XmlRootAttribute(rootAttribute));

            T currentClass = (T)Activator.CreateInstance(typeof(T));

            var typeDeserialization = (T[])serializer
            .Deserialize(new StringReader(inputXml));

            return typeDeserialization;
        }

        private static DateTime ConvertToDate(string date)
        {
            return DateTime.ParseExact(date, dFormat, CultureInfo.InvariantCulture);
        }




    }
}