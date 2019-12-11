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
            int size = int.Parse(Console.ReadLine());

            var field = new char[size][];

            for (int i = 0; i < field.Length; i++)
            {
                string input = Console.ReadLine();

                field[i] = input.ToCharArray();

                for (int j = 0; j < input.Length; j++)
                {
                    field[i][j] = input[j];
                }
            }
            string commands = Console.ReadLine();

            int[] samPosition = FindsSamPosition(field);
            int[] nikoladzePosition = FindsNikoladzesPosition(field);

            for (int i = 0; i < commands.Length; i++)
            {

                EnemyPatrol(field);
                bool enemySeesSam = ChekIfEnemiesSawSaw(field);
                if (enemySeesSam == true)
                {
                    Console.WriteLine($"Sam died at {samPosition[0]}, {samPosition[1]}");
                    field[samPosition[0]][samPosition[1]] = 'X';
                    Print(field);
                    break;
                }
                bool nikoladzeIsKilled = SamMoves(field, commands[i], samPosition, nikoladzePosition);
                if (nikoladzeIsKilled == true)
                {
                    Console.WriteLine("Nikoladze killed!");
                    field[nikoladzePosition[0]][nikoladzePosition[1]] = 'X';
                    Print(field);
                    break;
                }

            }


        }

        private static bool ChekIfEnemiesSawSaw(char[][] field)
        {
            for (var line = 0; line < field.Length; line++)
            {
                if (field[line].Contains('b') && field[line].Contains('S'))
                {
                    if (Array.IndexOf(field[line], 'b') < Array.IndexOf(field[line], 'S'))
                    {
                        return true;
                    }
                }
                else if (field[line].Contains('d') && field[line].Contains('S'))
                {
                    if (Array.IndexOf(field[line], 'd') > Array.IndexOf(field[line], 'S'))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool SamMoves(char[][] field, char command, int[] samPosition, int[] nikoladzePosition)
        {
            if (command == 'U')
            {
                if (samPosition[0] - 1 == nikoladzePosition[0])
                {
                    field[samPosition[0] - 1][samPosition[1]] = 'S';
                    field[samPosition[0]][samPosition[1]] = '.';
                    return true;
                }
                else
                {
                    if (field[samPosition[0] - 1][samPosition[1]] == 'b' || field[samPosition[0] - 1][samPosition[1]] == 'd')
                    {
                        field[samPosition[0] - 1][samPosition[1]] = 'S';
                        field[samPosition[0]][samPosition[1]] = '.';
                        samPosition[0] -= 1;
                    }
                    else
                    {
                        field[samPosition[0] - 1][samPosition[1]] = 'S';
                        field[samPosition[0]][samPosition[1]] = '.';
                        samPosition[0] -= 1;
                    }
                }
            }
            else if (command == 'D')
            {
                if (samPosition[0] + 1 == nikoladzePosition[0])
                {
                    field[samPosition[0] + 1][samPosition[1]] = 'S';
                    field[samPosition[0]][samPosition[1]] = '.';
                    return true;
                }
                else
                {
                    if (field[samPosition[0] + 1][samPosition[1]] == 'b' || field[samPosition[0] + 1][samPosition[1]] == 'd')
                    {
                        field[samPosition[0] + 1][samPosition[1]] = 'S';
                        field[samPosition[0]][samPosition[1]] = '.';
                        samPosition[0] += 1;

                    }
                    else
                    {
                        field[samPosition[0] + 1][samPosition[1]] = 'S';
                        field[samPosition[0]][samPosition[1]] = '.';
                        samPosition[0] += 1;
                    }
                }

            }
            else if (command == 'L')
            {
                field[samPosition[0]][samPosition[1] - 1] = 'S';
                field[samPosition[0]][samPosition[1]] = '.';
                samPosition[1] -= 1;
            }
            else if (command == 'R')
            {
                field[samPosition[0]][samPosition[1] + 1] = 'S';
                field[samPosition[0]][samPosition[1]] = '.';
                samPosition[1] += 1;
            }
            else if (command == 'W')
            {
                return false;
            }
            return false;
        }

        private static int[] FindsNikoladzesPosition(char[][] field)
        {
            int[] index = new int[2];
            for (int i = 0; i < field.Length; i++)
            {
                for (int j = 0; j < field[i].Length; j++)
                {
                    if (field[i][j] == 'N')
                    {
                        index[0] = i;
                        index[1] = j;
                        return index;
                    }
                }
            }
            return index;
        }

        private static void EnemyPatrol(char[][] field)
        {
            for (int i = 0; i < field.Length; i++)
            {
                for (int j = 0; j < field[i].Length; j++)
                {
                    if (field[i][j] == 'b')
                    {
                        if (j == field[i].Length - 1)
                        {
                            field[i][j] = 'd';
                        }
                        else
                        {
                            field[i][j] = '.';
                            field[i][++j] = 'b';
                        }
                    }
                    else if (field[i][j] == 'd')
                    {
                        if (j == 0)
                        {
                            field[i][j] = 'b';
                        }
                        else
                        {
                            field[i][j] = '.';
                            field[i][j - 1] = 'd';
                        }
                    }
                }
            }
        }

        private static int[] FindsSamPosition(char[][] field)
        {
            int[] indexes = new int[2];
            for (int i = 0; i < field.Length; i++)
            {
                for (int j = 0; j < field[i].Length; j++)
                {
                    if (field[i][j] == 'S')
                    {
                        indexes[0] = i;
                        indexes[1] = j;
                    }
                }
            }
            return indexes;
        }

        private static void Print(char[][] field)
        {
            for (int i = 0; i < field.Length; i++)
            {
                for (int j = 0; j < field[i].Length; j++)
                {
                    Console.Write($"{field[i][j]}");
                }
                Console.WriteLine();
            }
        }
    }
}