namespace PetClinic.App
{
    using System;
    using System.IO;
    
    using AutoMapper;

    using PetClinic.Data;

    public class StartUp
    {
        static void Main()
        {
            using (var context = new PetClinicContext())
            {
                Mapper.Initialize(config => config.AddProfile<PetClinicProfile>());

                ResetDatabase(context);

                ImportEntities(context);

                ExportEntities(context);
				
				BonusTask(context);
            }
        }

        private static void ImportEntities(PetClinicContext context, string baseDir = @"D:\SoftUni-Exercises\Entity Framework Core Exams\C#DBAdvancedRetakeExam-05.01.2018\01. Model Definition_Project Skeleton\Datasets\")
        {
            const string exportDir = "./Results/";

            string animalAids = DataProcessor.Deserializer.ImportAnimalAids(context, File.ReadAllText(baseDir + "animalAids.json"));
            Console.WriteLine(animalAids);
            PrintAndExportEntityToFile(animalAids, exportDir + "AnimalAidsImport.txt");

            string animals = DataProcessor.Deserializer.ImportAnimals(context, File.ReadAllText(baseDir + "animals.json"));
            Console.WriteLine(animals);
            PrintAndExportEntityToFile(animals, exportDir + "AnimalsImport.txt");

            string vets = DataProcessor.Deserializer.ImportVets(context, File.ReadAllText(baseDir + "vets.xml"));
            Console.WriteLine(vets);
            PrintAndExportEntityToFile(vets, exportDir + "VetsImport.txt");

            var procedures = DataProcessor.Deserializer.ImportProcedures(context, File.ReadAllText(baseDir + "procedures.xml"));
            Console.WriteLine(procedures);
            PrintAndExportEntityToFile(procedures, exportDir + "ProceduresImport.txt");
        }

        private static void ExportEntities(PetClinicContext context)
        {
            const string exportDir = "./Results/";

            string animalsExport = DataProcessor.Serializer.ExportAnimalsByOwnerPhoneNumber(context, "0887446123");
            Console.WriteLine(animalsExport);
            PrintAndExportEntityToFile(animalsExport, exportDir + "AnimalsExport.json");

            string proceduresExport = DataProcessor.Serializer.ExportAllProcedures(context);
            Console.WriteLine(proceduresExport);
            PrintAndExportEntityToFile(proceduresExport, exportDir + "ProceduresExport.xml");
        }
		
		private static void BonusTask(PetClinicContext context)
        {
            var bonusOutput = DataProcessor.Bonus.UpdateVetProfession(context, "+359284566778", "Primary Care");
            Console.WriteLine(bonusOutput);
        }

        private static void PrintAndExportEntityToFile(string entityOutput, string outputPath)
        {
            Console.WriteLine(entityOutput);
            File.WriteAllText(outputPath, entityOutput.TrimEnd());
        }

        private static void ResetDatabase(PetClinicContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Console.WriteLine("Database reset.");
        }
    }
}
