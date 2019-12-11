using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp87
{

    class Program
    {

        static void Main(string[] args)
        {
            int[] size = Console.ReadLine().Split().Select(int.Parse).ToArray();

            var field = new char[size[0]][];
            int[] numberOfHotels = new int[] { 0 };
            int[] currentMoney = new int[] { 50 };
            int[] totalTurns = new int[] { 0 };
            int[] currentTurn = new int[] { 0 };

            FillMatrix(field);

            for (int i = 0; i < field.Length; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < field[i].Length; j++)
                    {
                        PlayerMoves(field, i, j, numberOfHotels, currentMoney, totalTurns, currentTurn);
                    }
                }
                else
                {
                    for (int k = field[i].Length - 1; k >= 0; k--)
                    {
                        PlayerMoves(field, i, k, numberOfHotels, currentMoney, totalTurns, currentTurn);
                    }
                }
            }

            Console.WriteLine($"Turns {totalTurns[0]}");
            Console.WriteLine($"Money {currentMoney[0]}");

        }

        private static void PlayerMoves(char[][] field, int i, int j, int[] numberOfHotels, int[] currentMoney, int[] totalTurns, int[] currentTurn)
        {
            bool jailTime = false;
            switch (field[i][j])
            {
                case 'H':
                    numberOfHotels[0]++;
                    Console.WriteLine($"Bought a hotel for {currentMoney[0]}. Total hotels: {numberOfHotels[0]}.");
                    currentMoney[0] = 0;
                    break;
                case 'J':
                    Console.WriteLine($"Gone to jail at turn {currentTurn[0]}.");
                    currentTurn[0] += 2;
                    totalTurns[0] += 2;
                    jailTime = true;
                    break;
                case 'S':
                    int shopCost = (i + 1) * (j + 1);
                    if (shopCost > currentMoney[0])
                    {
                        Console.WriteLine($"Spent {currentMoney[0]} money at the shop.");
                        currentMoney[0] = 0;
                    }
                    else
                    {
                        currentMoney[0] -= shopCost;
                        Console.WriteLine($"Spent {shopCost} money at the shop.");
                    }
                    break;
                default:
                    break;
            }

            currentTurn[0]++;
            totalTurns[0]++;
            if (jailTime)
            {
                int hyqmi = numberOfHotels[0] * 2 * 10;
                currentMoney[0] += (numberOfHotels[0] * 10) + hyqmi;
            }
            else
            {
                currentMoney[0] += (numberOfHotels[0] * 10);
            }

        }

        private static void FillMatrix(char[][] field)
        {
            for (int i = 0; i < field.Length; i++)
            {
                string input = Console.ReadLine();
                field[i] = input.ToCharArray();
            }
        }
    }
}