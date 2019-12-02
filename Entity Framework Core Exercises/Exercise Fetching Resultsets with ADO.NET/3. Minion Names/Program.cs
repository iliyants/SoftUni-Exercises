using System;
using System.Data.SqlClient;
namespace _3._Minion_Names
{
    class Program
    {
        static void Main(string[] args)
        {
            int givenId = int.Parse(Console.ReadLine());

            using var connection = new SqlConnection
              ("Server=DESKTOP-FJ4UOL0\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True");

            connection.Open();

            string villainCommand =
                @$"SELECT Name FROM Villains WHERE Id = {givenId}";

            var selectVillainCommand = new SqlCommand(villainCommand, connection);

            string villainName = (string)selectVillainCommand.ExecuteScalar();

            if(villainName == null)
            {
                Console.WriteLine($"No villain with ID ${givenId} exists in the database.");
                return;
            }
            else
            {
                Console.WriteLine($"Villain : {villainName}");
            }

            string minionsQuery =
                $@"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = {givenId}
                                ORDER BY m.Name";

            var minionsCommand = new SqlCommand(minionsQuery, connection);

            var minionsReader = minionsCommand.ExecuteReader();
            var counter = 1;
            while(minionsReader.Read())
            {
                Console.WriteLine($"{counter}. {minionsReader["Name"]} {minionsReader["Age"]}");

                counter++;
            }          
        }
    }
}
