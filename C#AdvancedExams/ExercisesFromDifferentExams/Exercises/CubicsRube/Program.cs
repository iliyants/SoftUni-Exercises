using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CubicsRube
{

    class Program
    {

        static void Main(string[] args)
        {
            var size = long.Parse(Console.ReadLine());


            var cube = new long[size, size, size];

            while (true)
            {
                string input = Console.ReadLine();
                if (input == "Analyze")
                {
                    break;
                }
                long[] tokens = input.Split().Select(long.Parse).ToArray();

                long firstIndex = tokens[0];
                long secondIndex = tokens[1];
                long thirdIndex = tokens[2];
                long particles = tokens[3];

                IsInside(cube, firstIndex, secondIndex, thirdIndex, particles);

            }

            long sum = 0;
            int notChangedCells = 0;
            foreach (long cell in cube)
            {
                if (cell > 0)
                {
                    sum += cell;
                }
                else
                {
                    notChangedCells++;
                }
            }
            Console.WriteLine(sum);
            Console.WriteLine(notChangedCells);

        }


        private static void IsInside(long[,,] cube, long firstIndex, long secondIndex, long thirdIndex, long particles)
        {
            try
            {
                cube[firstIndex, secondIndex, thirdIndex] += particles;
            }
            catch (Exception)
            {
            }
        }
    }
}