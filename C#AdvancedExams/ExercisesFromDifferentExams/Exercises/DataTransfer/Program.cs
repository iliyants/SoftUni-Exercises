using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DefiningClasses
{
    public class StartUp
    {
        static void Main(string[] args)
        {

            int n = int.Parse(Console.ReadLine());
            var stack = new Stack<int>();
            string pattern =
                @"^(s:(?<!\s)(?<sender>[^;]+)(?!\s);r:(?<!\s)(?<receiver>[^;]+)(?!\s);m--""(?<message>[a-zA-Z\s]+)"")*$";


            string sender = string.Empty;
            string receiver = string.Empty;
            string message = string.Empty;
            string bothNames = string.Empty;
            int totalSum = 0;
            for (int i = 0; i < n; i++)
            {
                string input = Console.ReadLine();

                Match regex = Regex.Match(input, pattern);

                if (regex.Success)
                {
                    sender = regex.Groups["sender"].Value;
                    receiver = regex.Groups["receiver"].Value;
                    message = regex.Groups["message"].Value;
                    int currentSum = 0;
                    string resultFirstName = string.Empty;
                    string resultSecondName = string.Empty;
                    bothNames = $"{sender}{receiver}";
                    totalSum = DataProccess(sender, receiver, message, resultFirstName, resultSecondName, bothNames, currentSum, totalSum);
                }
                else
                {
                    continue;
                }



            }
            Console.WriteLine($"Total data transferred: {totalSum}MB");
        }

        private static int DataProccess(string sender, string receiver, string message, string resultFirstName, string resultSecondName, string bothNames, int currentSum, int totalSum)
        {
            for (int j = 0; j < bothNames.Length; j++)
            {
                if (char.IsNumber(bothNames[j]))
                {
                    currentSum += (int)Char.GetNumericValue(bothNames[j]);
                }
            }
            totalSum += currentSum;



            for (int p = 0; p < sender.Length; p++)
            {
                if (char.IsLetter(sender[p]) || sender[p] == ' ')
                {
                    resultFirstName += sender[p];
                }
            }

            for (int l = 0; l < receiver.Length; l++)
            {
                if (char.IsLetter(receiver[l]) || receiver[l] == ' ')
                {
                    resultSecondName += receiver[l];
                }
            }

            Console.WriteLine($"{resultFirstName} says \"{message}\" to {resultSecondName}");
            return totalSum;
        }
    }
}
