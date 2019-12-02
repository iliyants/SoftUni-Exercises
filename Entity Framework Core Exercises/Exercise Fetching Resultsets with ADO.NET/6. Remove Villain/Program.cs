using System;
using System.Data.SqlClient;

namespace _6._Remove_Villain
{
    class Program
    {
        static void Main(string[] args)
        {
            using var connection = new SqlConnection
              ("Server=DESKTOP-FJ4UOL0\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True");

            connection.Open();

            var villainId = int.Parse(Console.ReadLine());

            var villainIdQuery = $"SELECT Name FROM Villains WHERE Id = '{villainId}'";

            var selectIdCommand = new SqlCommand(villainIdQuery, connection);

            var villainName = (string)selectIdCommand.ExecuteScalar();

            if(villainName == null)
            {
                Console.WriteLine($"No such villain was found.");
                return;
            }

            var releaseMinionsQuery = @$"DELETE FROM MinionsVillains 
                                        WHERE VillainId = '{villainId}'";

            var releaseMinionsCommand = new SqlCommand(releaseMinionsQuery, connection);

            var minionsReleased = (int)releaseMinionsCommand.ExecuteNonQuery();

            var deleteVillainQuery = $@"DELETE FROM Villains
                                     WHERE Id = '{villainId}'";

            Console.WriteLine($"{villainName} was deleted.");
            Console.WriteLine($"{minionsReleased} minions were released.");


        }
    }
}
