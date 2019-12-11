using System;
using System.Linq;

namespace tron
{
    class Startup
    {
        static void Main(string[] args)
        {
            int size = int.Parse(Console.ReadLine());

            var matrix = new char[size][];

            var firstPlayerIndexes = new int[2];
            var secondPlayerIndexes = new int[2];

            for (int i = 0; i < matrix.Length; i++)
            {
                var input = Console.ReadLine();
                matrix[i] = input.ToCharArray();
                if (matrix[i].Contains('f'))
                {
                    firstPlayerIndexes = new int[] { i, Array.IndexOf(matrix[i], 'f') };
                }
                else if (matrix[i].Contains('s'))
                {
                    secondPlayerIndexes = new int[] { i, Array.IndexOf(matrix[i], 's') };
                }
            }


            while (true)
            {

                var commands = Console.ReadLine().Split().ToArray();

                var firstPlayerCommand = commands[0];
                var secondPlayerCommand = commands[1];

                switch (firstPlayerCommand)
                {
                    case "up":
                        firstPlayerIndexes[0]--; break;
                    case "down":
                        firstPlayerIndexes[0]++; break;
                    case "left":
                        firstPlayerIndexes[1]--; break;
                    case "right":
                        firstPlayerIndexes[1]++; break;
                    default:
                        break;
                }
                if (firstPlayerIndexes[0] < 0)
                {
                    firstPlayerIndexes[0] = matrix.Length - 1;
                }
                else if (firstPlayerIndexes[0] > matrix.Length - 1)
                {
                    firstPlayerIndexes[0] = 0;
                }
                else if (firstPlayerIndexes[1] < 0)
                {
                    firstPlayerIndexes[1] = matrix[0].Length - 1;
                }
                else if (firstPlayerIndexes[1] > matrix.Length - 1)
                {
                    firstPlayerIndexes[1] = 0;
                }

                if (matrix[firstPlayerIndexes[0]][firstPlayerIndexes[1]] == 's')
                {
                    matrix[firstPlayerIndexes[0]][firstPlayerIndexes[1]] = 'x';
                    break;
                }
                else
                {
                    matrix[firstPlayerIndexes[0]][firstPlayerIndexes[1]] = 'f';
                }


                switch (secondPlayerCommand)
                {
                    case "up":
                        secondPlayerIndexes[0]--; break;
                    case "down":
                        secondPlayerIndexes[0]++; break;
                    case "left":
                        secondPlayerIndexes[1]--; break;
                    case "right":
                        secondPlayerIndexes[1]++; break;
                    default:
                        break;
                }
                if (secondPlayerIndexes[0] < 0)
                {
                    secondPlayerIndexes[0] = matrix.Length - 1;
                }
                else if (secondPlayerIndexes[0] > matrix.Length - 1)
                {
                    secondPlayerIndexes[0] = 0;
                }
                else if (secondPlayerIndexes[1] < 0)
                {
                    secondPlayerIndexes[1] = matrix[0].Length - 1;
                }
                else if (secondPlayerIndexes[1] > matrix.Length - 1)
                {
                    secondPlayerIndexes[1] = 0;
                }

                if (matrix[secondPlayerIndexes[0]][secondPlayerIndexes[1]] == 'f')
                {
                    matrix[secondPlayerIndexes[0]][secondPlayerIndexes[1]] = 'x';
                    break;
                }
                else
                {
                    matrix[secondPlayerIndexes[0]][secondPlayerIndexes[1]] = 's';
                }


            }

            foreach (var item in matrix)
            {
                Console.WriteLine(string.Join("", item));
            }

        }
    }
}