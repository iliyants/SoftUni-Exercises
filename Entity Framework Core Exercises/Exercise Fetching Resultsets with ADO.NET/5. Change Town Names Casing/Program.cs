using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace _5._Change_Town_Names_Casing
{
    class Program
    {
        static void Main(string[] args)
        {
              using var connection = new SqlConnection
              ("Server=DESKTOP-FJ4UOL0\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True");

            connection.Open();

           var countryName = Console.ReadLine();

            var updateQuery = @$"UPDATE Towns
                        SET Name = UPPER(Name)
                        WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = '{countryName}')";

            var selectTownsQuery = @$" 
                            SELECT t.Name 
                            FROM Towns as t
                            JOIN Countries AS c ON c.Id = t.CountryCode
                            WHERE c.Name = '{countryName}'";

               var updateCommand = new SqlCommand(updateQuery, connection);

            try
            {
                var linesAffected =  (int)updateCommand.ExecuteNonQuery();
                Console.WriteLine($"{linesAffected} town names were affected.");

                 var selectTownsCommand = new SqlCommand(selectTownsQuery, connection);

                 var townsReader = selectTownsCommand.ExecuteReader();
                 var townStorage = new List<string>();

                while(townsReader.Read())
                {
                    townStorage.Add((string)townsReader["Name"]);
                }
                
                Console.WriteLine($"[{String.Join(", ", townStorage.ToList())}]");

            }
            catch
            {
                Console.WriteLine("No town names were affected.");
            }
           
        }
    }
}
