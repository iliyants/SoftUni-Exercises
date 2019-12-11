namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ExportDto;
    using TeisterMask.DataProcessor.ImportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static IFormatProvider CUltureInfo { get; private set; }

        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {

            var projects = context.Projects
                .Where(x => x.Tasks.Any())
                .Select(x => new ExportProjectDTO()
                {
                    TasksCount = x.Tasks.Count(),
                    ProjectName = x.Name,
                    HasEndDate = CheckEndDate(x.DueDate),
                    Tasks = x.Tasks.Select(t => new ExportTaskDTO()
                    {
                        Name = t.Name,
                        LabelType = t.LabelType.ToString()
                    })
                    .OrderBy(n => n.Name)
                    .ToArray()

                })
                .OrderByDescending(x => x.TasksCount)
                .ThenBy(x => x.ProjectName)
                .ToArray();

            return SerializeCollectionToXML("Projects", projects);

        }

        private static string CheckEndDate(DateTime? dueDate)
        {
            if (dueDate == null)
            {
                return "No";
            }

            return "Yes";
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context
                .Employees
                .Where(x => x.EmployeesTasks.Any(t => ValidDate(t.Task.OpenDate, date)))
                .Select(x => new
                {
                    Username = x.Username,

                    Tasks = x.EmployeesTasks
                    .Where(vd => ValidDate(vd.Task.OpenDate, date))
                    .OrderByDescending(dd => dd.Task.DueDate)
                    .ThenBy(n => n.Task.Name)
                        .Select(t => new
                        {
                            TaskName = t.Task.Name,
                            OpenDate = t.Task.OpenDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                            DueDate = t.Task.DueDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                            LabelType = t.Task.LabelType.ToString(),
                            ExecutionType = t.Task.ExecutionType.ToString()

                        })
                })
                .OrderByDescending(tc => tc.Tasks.Count())
                .ThenBy(un => un.Username)
                .Take(10);



            var jsonString = JsonConvert.SerializeObject(employees, Newtonsoft.Json.Formatting.Indented);

            return jsonString;
        }

        private static List<string> returnValidEmployees(TeisterMaskContext context, DateTime date)
        {
            var result = new List<string>();

            foreach (var employee in context.Employees)
            {
                foreach (var task in employee.EmployeesTasks)
                {
                    if (ValidDate(task.Task.OpenDate, date))
                    {
                        result.Add(employee.Username);
                        break;
                    }
                }
            }

            return result;
        }

        private static bool ValidDate(DateTime openDate, DateTime date)
        {

            var comparison = DateTime.Compare(openDate, date);

            if (comparison < 0)
            {
                return false;
            }

            return true;
        }

        public static string SerializeCollectionToXML<T>(string rootAttribute, T[] collection)
        {
            var serializer = new XmlSerializer(typeof(T[]),
                       new XmlRootAttribute(rootAttribute));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), collection, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}