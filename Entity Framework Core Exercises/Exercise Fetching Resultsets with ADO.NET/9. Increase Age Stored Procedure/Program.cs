using System;
using System.Data.SqlClient;

namespace _9._Increase_Age_Stored_Procedure
{
    class Program
    {
        static void Main(string[] args)
        {
            using var connection = new SqlConnection
           ("Server=DESKTOP-FJ4UOL0\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True");

            connection.Open();

            var minionId = int.Parse(Console.ReadLine());

            var updateQuery = $@"EXEC dbo.usp_GetOlder {minionId}";

            var updateCommand = new SqlCommand(updateQuery, connection);

            updateCommand.ExecuteNonQuery();

            var selectQuery = $@"SELECT Name, Age FROM Minions WHERE Id = {minionId}";

            var selectCommand = new SqlCommand(selectQuery, connection);

            var reader = selectCommand.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]} - {reader["Age"]} years old");
            }

        }
    }
}
