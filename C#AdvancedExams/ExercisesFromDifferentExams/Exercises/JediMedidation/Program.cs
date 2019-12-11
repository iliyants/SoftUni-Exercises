using System;
using System.Collections.Generic;
using System.Linq;

namespace JediMeditation
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());

            var masters = new Queue<string>();
            var knights = new Queue<string>();
            var padwans = new Queue<string>();
            var slavToshko = new Queue<string>();

            bool yodaIsHere = false;

            for (int i = 0; i < n; i++)
            {
                var input = Console.ReadLine();

                FIllingQueues(masters, knights, padwans, slavToshko, input);
                if (input.Contains('y'))
                {
                    yodaIsHere = true;
                }
            }

            if (yodaIsHere)
            {
                Console.WriteLine(string.Join(" ", masters) + " " +
                    string.Join(" ", knights) + " " +
                    string.Join(" ", slavToshko) + " " +
                    string.Join(" ", padwans));
            }
            else
            {
                Console.WriteLine(string.Join(" ", slavToshko) + " " +
                    string.Join(" ", masters) + " " +
                    string.Join(" ", knights) + " " +
                    string.Join(" ", padwans));
            }




        }

        private static void FIllingQueues(Queue<string> masters, Queue<string> knights, Queue<string> padwans, Queue<string> slavToshko, string input)
        {
            var split = input.Split().ToArray();

            for (int i = 0; i < split.Length; i++)
            {
                if (split[i].Contains("m"))
                {
                    masters.Enqueue(split[i]);
                }
                else if (split[i].Contains("k"))
                {
                    knights.Enqueue(split[i]);
                }
                else if (split[i].Contains("p"))
                {
                    padwans.Enqueue(split[i]);
                }
                else if (split[i].Contains("s") || split[i].Contains("t"))
                {
                    slavToshko.Enqueue(split[i]);
                }
            }
        }
    }
}