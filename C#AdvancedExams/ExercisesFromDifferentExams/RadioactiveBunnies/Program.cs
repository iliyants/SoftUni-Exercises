using System;
using System.Collections.Generic;
using System.Linq;

namespace RadioactiveMutantVampireBunnies
{
    class Program
    {
        static void Main(string[] args)
        {
            var size = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .ToArray();

            var matrix = new char[size[0]][];
            var playerCoord = new int[2];


            for (int i = 0; i < matrix.Length; i++)
            {
                var input = Console.ReadLine();
                matrix[i] = input.ToCharArray();
                if (matrix[i].Contains('P'))
                {
                    playerCoord = new int[] { i, Array.IndexOf(matrix[i], 'P') };
                }
            }

            var commands = Console.ReadLine();

            var gameOver = new bool[1] { false };
            bool playeReachedABunny = false;
            bool won = false;

            foreach (var command in commands)
            {
                matrix[playerCoord[0]][playerCoord[1]] = '.';
                int previousRow = playerCoord[0];
                int previousCol = playerCoord[1];

                switch (command)
                {
                    case 'U': playerCoord[0]--; break;
                    case 'D': playerCoord[0]++; break;
                    case 'L': playerCoord[1]--; break;
                    case 'R': playerCoord[1]++; break;
                    default:
                        break;
                }

                bool playerIsInside = ChecksIfThePlayerIsInside(playerCoord, matrix);

                if (playerIsInside)
                {
                    playeReachedABunny = ChecksIfThePlayerReachedABunny(playerCoord, matrix);

                    if (playeReachedABunny)
                    {
                        gameOver[0] = true;
                    }
                    else
                    {
                        matrix[playerCoord[0]][playerCoord[1]] = 'P';
                    }
                }
                else
                {
                    playerCoord[0] = previousRow;
                    playerCoord[1] = previousCol;
                    gameOver[0] = true;
                    won = true;
                }

                bool bunniesReachedPlayer = BunnySpread(playerCoord, matrix, gameOver);

                if (bunniesReachedPlayer || gameOver[0])
                {
                    break;
                }
            }

            Print(matrix);

            if (won)
            {
                Console.WriteLine($"won: {playerCoord[0]} {playerCoord[1]}");
            }
            else
            {
                Console.WriteLine($"dead: {playerCoord[0]} {playerCoord[1]}");
            }

        }

        private static void Print(char[][] matrix)
        {
            foreach (var item in matrix)
            {
                Console.WriteLine(string.Join("", item));
            }
        }

        private static bool BunnySpread(int[] playerCoord, char[][] matrix, bool[] gameOver)
        {
            var bunnyIndexes = new Queue<int[]>();

            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (matrix[i][j] == 'B')
                    {
                        bunnyIndexes.Enqueue(new int[] { i, j });
                    }
                }
            }

            while (bunnyIndexes.Any())
            {
                var currentIndexes = bunnyIndexes.Dequeue();
                var bunnyRow = currentIndexes[0];
                var bunnyCol = currentIndexes[1];

                if (bunnyRow + 1 < matrix.Length)
                {
                    matrix[bunnyRow + 1][bunnyCol] = 'B';
                }
                if (bunnyRow - 1 >= 0)
                {
                    matrix[bunnyRow - 1][bunnyCol] = 'B';
                }
                if (bunnyCol + 1 < matrix[0].Length)
                {
                    matrix[bunnyRow][bunnyCol + 1] = 'B';
                }
                if (bunnyCol - 1 >= 0)
                {
                    matrix[bunnyRow][bunnyCol - 1] = 'B';
                }
            }

            if (matrix[playerCoord[0]][playerCoord[1]] == 'B' && !gameOver[0])
            {
                gameOver[0] = true;
                return true;
            }
            return false;
        }

        private static bool ChecksIfThePlayerReachedABunny(int[] playerCoord, char[][] matrix)
        {
            if (matrix[playerCoord[0]][playerCoord[1]] == 'B')
            {
                return true;
            }
            matrix[playerCoord[0]][playerCoord[1]] = 'P';
            return false;
        }

        private static bool ChecksIfThePlayerIsInside(int[] playerCoord, char[][] matrix)
        {
            if (playerCoord[0] < matrix.Length && playerCoord[0] >= 0 &&
                playerCoord[1] < matrix[0].Length && playerCoord[1] >= 0)
            {
                return true;
            }
            return false;
        }
    }
}