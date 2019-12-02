namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departmentsDTO = JsonConvert.DeserializeObject<ImportDepartmentDTO[]>(jsonString);

            var sb = new StringBuilder();

            foreach (var departmentDTO in departmentsDTO)
            {
                var validDepartment = IsValid(departmentDTO);
                var validCells = departmentDTO.Cells.All(IsValid);

                if (!validDepartment || !validCells)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var department = AutoMapper.Mapper.Map<Department>(departmentDTO);

                context.Departments.Add(department);

                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count()} cells");

                context.SaveChanges();
            }

            return sb.ToString();

               
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var prisonersDTO = JsonConvert.DeserializeObject<ImportPrisonerDTO[]>(jsonString);

            var sb = new StringBuilder();

            foreach (var prisonerDTO in prisonersDTO)
            {
                var validPrisoner = IsValid(prisonerDTO);
                var validMails = prisonerDTO.Mails.All(IsValid);

                if (!validPrisoner || !validMails)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var prisoner = AutoMapper.Mapper.Map<Prisoner>(prisonerDTO);

                context.Prisoners.Add(prisoner);

                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");

                context.SaveChanges();

            }

            return sb.ToString();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var officersDTO = DeserializedCollection<ImportOfficerDTO>("Officers", xmlString);

            var sb = new StringBuilder();

            foreach (var officerDTO in officersDTO)
            {
                var validOfficer = IsValid(officerDTO);
                var validWeapon = Enum.TryParse(officerDTO.Weapon, out Weapon weaponResult);
                var validPosition = Enum.TryParse(officerDTO.Position, out Position positionResult);

                if (!validOfficer || 
                    !validWeapon || 
                    !validPosition )
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var officer = AutoMapper.Mapper.Map<Officer>(officerDTO);

                foreach (var prisoner in officerDTO.PrisonerIds)
                {
                    var officerPrisoners = new OfficerPrisoner()
                    {
                        OfficerId = officer.Id,
                        PrisonerId = prisoner.Id
                    };

                    officer.OfficerPrisoners.Add(officerPrisoners);
                }

                context.Officers.Add(officer);

                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count()} prisoners)");
            }

            context.SaveChanges();
            return sb.ToString();
        }


        private static bool IsValid(object model)
        {
            var validationContext = new ValidationContext(model);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(model, validationContext, validationResult, true);
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
    }


}