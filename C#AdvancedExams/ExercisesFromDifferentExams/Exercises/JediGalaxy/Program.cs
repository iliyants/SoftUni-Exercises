using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{


    static void Main()
    {



        int[] size = Console.ReadLine().Split().Select(int.Parse).ToArray();

        int counter = 0;

        var matrix = new int[size[0], size[1]];

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] = counter;
                counter++;
            }
        }

        var jediCollection = new List<int>();
        var deadStars = new List<int>();
        long result = 0;
        while (true)
        {
            string input = Console.ReadLine();
            if (input == "Let the Force be with you")
            {
                break;
            }

            int[] jedi = input.Split().Select(int.Parse).ToArray();
            int[] dark = Console.ReadLine().Split().Select(int.Parse).ToArray();

            int jediRow = jedi[0];
            int jediCol = jedi[1];

            int darkRow = dark[0];
            int darkCol = dark[1];

            if (darkRow >= matrix.GetLength(0))
            {
                int value = darkRow - matrix.GetLength(0) + 1;
                darkRow -= value;
                darkCol -= value;
            }

            if (darkCol >= matrix.GetLength(1))
            {
                int value = darkCol - matrix.GetLength(1) + 1;
                darkRow -= value;
                darkCol -= value;
            }

            while (darkRow >= 0 && darkCol >= 0)
            {
                matrix[darkRow, darkCol] = 0;
                darkRow--;
                darkCol--;
            }

            //JEDI
            if (jediRow >= matrix.GetLength(0))
            {
                int value = jediRow - matrix.GetLength(0) + 1;
                jediRow -= value;
                jediCol += value;
            }

            if (jediCol < 0)
            {
                int value = Math.Abs(jediCol);
                jediRow -= value;
                jediCol += value;
            }

            while (jediRow >= 0 && jediCol < matrix.GetLength(1))
            {
                result += matrix[jediRow, jediCol];
                jediRow--;
                jediCol++;
            }

        }

        Console.WriteLine(result);
    }

    private static void Print(int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write($"{matrix[i, j]} ");
            }
            Console.WriteLine();
        }
    }
}
