using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelFunctions
{
    class Startup
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());

            var head = Console.ReadLine().Split(", ").ToList();

            var matrix = new List<List<string>>();

            for (int i = 0; i < n - 1; i++)
            {
                var input = Console.ReadLine().Split(", ").ToList();
                matrix.Add(new List<string>());
                for (int j = 0; j < head.Count; j++)
                {
                    matrix[i].Add(input[j]);
                }
            }

            var commands = Console.ReadLine().Split();
            string header = commands[1];
            int index = head.FindIndex(x => x == header);

            if (commands[0] == "hide")
            {
                for (int i = 0; i < n - 1; i++)
                {
                    matrix[i].RemoveAt(index);
                }
                head.Remove(header);
                Console.WriteLine(string.Join(" | ", head));
                foreach (var item in matrix)
                {
                    Console.WriteLine(string.Join(" | ", item));
                }
            }
            else if (commands[0] == "sort")
            {
                var sortedMatrix = matrix.OrderBy(x => x[index]).ToList();
                Console.WriteLine(string.Join(" | ", head));
                foreach (var item in sortedMatrix)
                {
                    Console.WriteLine(string.Join(" | ", item));
                }
            }
            else if (commands[0] == "filter")
            {
                string value = commands[2];
                var selection = new List<int>();

                for (int i = 0; i < n - 1; i++)
                {
                    if (matrix[i][index] == value)
                    {
                        selection.Add(i);
                    }
                }
                Console.WriteLine(string.Join(" | ", head));
                for (int i = 0; i < selection.Count; i++)
                {
                    Console.WriteLine(string.Join(" | ", matrix[selection[i]].ToList()));
                }

            }
        }
    }
}