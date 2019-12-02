using System;
using System.Data.SqlClient;
namespace _4._Add_Minion
{
    class Program
    {
        static void Main(string[] args)
        {
            using var connection = new SqlConnection
               ("Server=DESKTOP-FJ4UOL0\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True");

            connection.Open();

            Console.Write("Minion: ");

            var minionParameters = Console.ReadLine().Split(' ');
            var minionName = minionParameters[0];
            var minionAge = int.Parse(minionParameters[1]);
            var townName = minionParameters[2];

            Console.Write("Villain: ");

            var villainName = Console.ReadLine();

            var selectVillainId = @$"SELECT Id
                                    FROM Villains 
                                    WHERE Name = '{villainName}'";

            var selectTownId = $@"SELECT Id 
                                  FROM Towns
                                  WHERE Name = '{townName}'";

            var selectMinionId = @$"SELECT Id 
                                    FROM Minions  
                                    WHERE Name = '{minionName}'";

            int? villainId = ExecuteScalarCommandForInt(selectVillainId, connection);

            if(villainId == null)
            {
                villainId = AddVillain(villainName, selectVillainId, connection);
                if(villainId == null)
                {
                    Console.WriteLine("Something went terribly wrong, please try again later");
                    return;
                }
                else
                {
                    Console.WriteLine($"Villain with name {villainName} and id {villainId} was successfully added in the database.");
                }
            }

            int? townId = ExecuteScalarCommandForInt(selectTownId, connection);

            if (townId == null)
            {
                townId = AddTown(townName, selectTownId, connection);
                if (townId == null)
                {
                    Console.WriteLine("Something went terribly wrong, please try again later");
                    return;
                }
                else
                {
                    Console.WriteLine($"Town with name {townName} and id {townId} was successfully added in the database.");
                }
            }

            int? minionId = ExecuteScalarCommandForInt(selectMinionId, connection);

            if(minionId == null)
            {
                minionId = AddMinion(minionName, minionAge, (int)townId, selectMinionId, connection);
                if(minionId == null)
                {
                    Console.WriteLine("Something went terribly wrong, please try again later");
                    return;
                }
                else
                {
                    Console.WriteLine($"Minion with name {minionName} and Id {minionId} and town Id {townId} was successfully added in the database.");

                }
            }

            try
            {
                AddMinionToVIllain((int)minionId, (int)villainId, connection);
                Console.WriteLine($"Minion with name {minionName} is now a servant to villain {villainName}");
            }
            catch
            {
                Console.WriteLine($"Minion {minionName} is already a servant to villain {villainName}.");
                return;
            }





        }

        private static void ExecuteNonQueryCommand(string query, SqlConnection connection)
        {
            var command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();

        }

        private static int? ExecuteScalarCommandForInt(string query, SqlConnection connection)
        {
            var command = new SqlCommand(query, connection);

            int? value = (int?)command.ExecuteScalar();

            return value;
        }

        private static int? AddVillain(string villainName,string selectVillainIdQuery,SqlConnection connection)
        {
            var query = $@"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES ('{villainName}', 4)";

            ExecuteNonQueryCommand(query, connection);

            int? villainId = ExecuteScalarCommandForInt(selectVillainIdQuery, connection);

            return villainId;

        }

        private static int? AddTown(string townName,string selectTownIdQuery,SqlConnection connection)
        {
            var query = $"INSERT INTO Towns(Name) VALUES('{townName}')";

            ExecuteNonQueryCommand(query, connection);

            int? townId = ExecuteScalarCommandForInt(selectTownIdQuery, connection);

            return townId;

        }

        private static int? AddMinion(string minionName,int minionAge,int townId,string selectMinionIdQuery,SqlConnection connection)
        {
            var query = $"INSERT INTO Minions (Name, Age, TownId) VALUES ('{minionName}', '{minionAge}', '{townId}')";

            ExecuteNonQueryCommand(query, connection);

            int? minionId = ExecuteScalarCommandForInt(selectMinionIdQuery, connection);

            return minionId;

        }

        private static void AddMinionToVIllain(int minionId,int villainId,SqlConnection connection)
        {
            var query = $"INSERT INTO MinionsVillains(MinionId, VillainId) VALUES('{minionId}', '{villainId}')";

            ExecuteNonQueryCommand(query, connection);
        }


    }
}
