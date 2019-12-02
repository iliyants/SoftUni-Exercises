using System;
using System.Data.SqlClient;
using System.Linq;

namespace _8._Increase_Minion_Age
{
    class Program
    {
        static void Main(string[] args)
        {

            using var connection = new SqlConnection
             ("Server=DESKTOP-FJ4UOL0\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True");

            connection.Open();

            var minionsIds = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .ToArray();

            for (int i = 0; i < minionsIds.Length - 1; i++)
            {
                var updateQuery = $@" UPDATE Minions
                            SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                            WHERE Id = {minionsIds[i]}";

                var updateCommand = new SqlCommand(updateQuery, connection);

                updateCommand.ExecuteNonQuery();
            }

            var selectQuery = $@"SELECT Name, Age FROM Minions";

            var selectCommand = new SqlCommand(selectQuery, connection);

            var reader = selectCommand.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]} {reader["Age"]}");
            }

          
        }
    }
}
