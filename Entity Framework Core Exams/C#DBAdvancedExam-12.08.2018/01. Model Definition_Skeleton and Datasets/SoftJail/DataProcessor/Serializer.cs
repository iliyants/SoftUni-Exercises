namespace SoftJail.DataProcessor
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = new List<ExportPrisonerWithCellDTO>();

            foreach (var id in ids)
            {
                var prisoner = context.Prisoners
                    .Where(x => x.Id == id)
                    .Select(x => new ExportPrisonerWithCellDTO()
                    {
                        Id = x.Id,
                        Name = x.FullName,
                        CellNumber = x.Cell.CellNumber,

                        Officers = x.PrisonerOfficers.Select(po => new ExportOfficerDTO()
                        {
                            OfficerName = po.Officer.FullName,
                            Department = po.Officer.Department.Name
                        })
                        .OrderBy(x => x.OfficerName)
                        .ToArray(),

                        TotalOfficerSalary = x.PrisonerOfficers.Sum(s => s.Officer.Salary)
                    }).FirstOrDefault();
                
                prisoners.Add(prisoner);
            }

            var jsonString = JsonConvert.SerializeObject
                (prisoners.OrderBy(x => x.Name)
                .ThenBy(x => x.Id), Newtonsoft.Json.Formatting.Indented);

            return jsonString;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var names = prisonersNames.Split(",").ToArray();
            var prisoners = new List<ExportPrisonerMessageDTO>();
            foreach (var name in names)
            {
                var prisoner = context.Prisoners
                    .Where(x => x.FullName == name)
                    .Select(x => new ExportPrisonerMessageDTO()
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        IncarcerationDate = x.IncarcerationDate.ToString("yyyy-MM-dd"),

                        Messages = x.Mails.Select(m => new ExportEncryptedMessageDTO()
                        {
                            Description = ReverseString(m.Description)
                        })
                       .ToArray()
                    }).FirstOrDefault();

                prisoners.Add(prisoner);
            }

            return SerializeCollectionToXML
                ("Prisoners", prisoners.OrderBy(x => x.FullName).ThenBy(x => x.Id).ToArray());
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

        public static string ReverseString(string text)
        {
            char[] arr = text.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
    }
}