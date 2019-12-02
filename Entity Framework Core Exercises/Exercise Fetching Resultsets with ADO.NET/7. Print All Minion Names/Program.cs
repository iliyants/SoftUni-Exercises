using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace _7._Print_All_Minion_Names
{
    class Program
    {
        static void Main(string[] args)
        {
            
            using var connection = new SqlConnection
              ("Server=DESKTOP-FJ4UOL0\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True");

            connection.Open();

            //custom table for better reality check with the document
            var selectNamesQuery = $@"SELECT Name FROM MinionsNamesTest";

            var minionsNamesCommand = new SqlCommand(selectNamesQuery, connection);

            var namesReader = minionsNamesCommand.ExecuteReader();

            var names = new List<string>();

            while (namesReader.Read())
            {
                names.Add((string)namesReader["Name"]);
            }
            var topAdder = 0;
            var bottomSubstractor = names.Count - 1;
            for (int i = 0; i <= names.Count - 1; i++)
            {
                if (i % 2 == 0)
                {
                    Console.WriteLine(names[topAdder++]);
                }
                else
                {
                    Console.WriteLine(names[bottomSubstractor--]);
                }
            }
        }
    }
}
