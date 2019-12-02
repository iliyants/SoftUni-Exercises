using System;
using System.Data.SqlClient;
namespace _2._Villain_Names
{
    class StartUp
    {
        static void Main(string[] args)
        {
            using var connection = new SqlConnection
               ("Server=DESKTOP-FJ4UOL0\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True");

            connection.Open();

            string selectQuery =

                        @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount
                     FROM Villains AS v
                     JOIN MinionsVillains AS mv ON v.Id = mv.VillainId
                     GROUP BY v.Id, v.Name
                     HAVING COUNT(mv.VillainId) > 3
                     ORDER BY COUNT(mv.VillainId)";


            var command = new SqlCommand(selectQuery, connection);

            var reader = command.ExecuteReader();

            while(reader.Read())
            {
                string name = (string)reader["Name"];
                int count = (int)reader["MinionsCount"];

                Console.WriteLine($"{name} - {count}");
            }
           

        }
    }
}
