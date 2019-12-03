namespace PetClinic.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using PetClinic.Data;
    using PetClinic.DataProcessor.ImportDTOs;
    using PetClinic.Models;

    public class Deserializer
    {

        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            var animalAidsDTO = JsonConvert.DeserializeObject<ImportAnimalAidDTO[]>(jsonString);

            var sb = new StringBuilder();

            foreach (var animalAidDTO in animalAidsDTO)
            {
                var animalAidIsValid = IsValid(animalAidDTO);
                var animalAidAlreadyExists = context.AnimalAids.Any(x => x.Name == animalAidDTO.Name);

                if (!animalAidIsValid || animalAidAlreadyExists)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var animal = AutoMapper.Mapper.Map<AnimalAid>(animalAidDTO);
                context.AnimalAids.Add(animal);
                context.SaveChanges();

                sb.AppendLine($"Record {animalAidDTO.Name} successfully imported.");
            }

            return sb.ToString();
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            var animalsPassportsDTO = JsonConvert.DeserializeObject<ImportAnimalDTO[]>(jsonString);

            var sb = new StringBuilder();

            foreach (var animalPassportDTO in animalsPassportsDTO)
            {
                var validAnimal = IsValid(animalPassportDTO);
                var validPassport = IsValid(animalPassportDTO.Passport);
                var passportExists = context.Passports.Any(x => x.SerialNumber == animalPassportDTO.Passport.SerialNumber);

                if (!validAnimal || !validPassport || passportExists)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var animal = AutoMapper.Mapper.Map<Animal>(animalPassportDTO);
                var passport = AutoMapper.Mapper.Map<Passport>(animalPassportDTO.Passport);

                context.Animals.Add(animal);
                context.Passports.Add(passport);
                context.SaveChanges();

                sb
                .AppendLine($"Record {animalPassportDTO.Name} Passport №: {animalPassportDTO.Passport.SerialNumber} successfully imported.");
            }

            return sb.ToString();
        }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            var vetsDTO = DeserializedCollection<ImportVetDTO>("Vets",xmlString);

            var sb = new StringBuilder();

            foreach (var vetDTO in vetsDTO)
            {
                var validVet = IsValid(vetDTO);
                var phoneNumberExists = context.Vets.Any(x => x.PhoneNumber == vetDTO.PhoneNumber);

                if (!validVet || phoneNumberExists)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var vet = AutoMapper.Mapper.Map<Vet>(vetDTO);
                context.Vets.Add(vet);
                context.SaveChanges();

                sb.AppendLine($"Record {vetDTO.Name} successfully imported.");
            }

            return sb.ToString();
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            var prodecuresDTO = DeserializedCollection<ImportProcedureDTO>("Procedures",xmlString);

            var sb = new StringBuilder();

            var procedureAnimalAids = new List<ProcedureAnimalAid>();

            foreach (var procedureDTO in prodecuresDTO)
            {
                var vetNameExists = context.Vets.Any(x => x.Name == procedureDTO.VetName);
                var animalSerialNumberExists = context.Animals.Any(x => x.PassportSerialNumber == procedureDTO.AnimalSerialNumber);
                var allAnimalAidsExist = procedureDTO.AnimalAids.Any(x => context.AnimalAids.Any(y => y.Name.Equals(x.Name)));
                var animalAidsAreUnique = 
                    procedureDTO.AnimalAids.Select(x => x.Name).ToArray().Distinct().Count() == procedureDTO.AnimalAids.Select(x => x.Name).ToArray().Count();

                if (!vetNameExists || !animalSerialNumberExists || !allAnimalAidsExist || !animalAidsAreUnique)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var vet = context.Vets.FirstOrDefault(x => x.Name == procedureDTO.VetName);
                var animal = context.Animals.FirstOrDefault(x => x.PassportSerialNumber == procedureDTO.AnimalSerialNumber);
                var dateTime = DateTime.ParseExact(procedureDTO.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                var procedure = new Procedure()
                {
                    Vet = vet,
                    Animal = animal,
                    DateTime = dateTime,
                };


                foreach (var animalAid in procedureDTO.AnimalAids)
                {
                    var currentAnimalAid = context.AnimalAids.FirstOrDefault(x => x.Name == animalAid.Name);

                    var procedureAnimalAid = new ProcedureAnimalAid()
                    {
                        Procedure = procedure,
                        AnimalAid = currentAnimalAid
                    };

                    context.ProceduresAnimalAids.Add(procedureAnimalAid);

                }

                context.Procedures.Add(procedure);
                context.SaveChanges();
                sb.AppendLine("Record successfully imported.");

            }

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
