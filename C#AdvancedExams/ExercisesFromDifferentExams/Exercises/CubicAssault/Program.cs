using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace CubicAssault
{



    class Program
    {
        static void Main(string[] args)
        {

            var dict = new Dictionary<string, Dictionary<string, BigInteger>>();

            while (true)
            {
                string input = Console.ReadLine();
                if (input == "Count em all")
                {
                    break;
                }
                string[] tokens = Regex.Split(input, @"\s+\-\>\s+");
                string region = tokens[0];
                string color = tokens[1];
                BigInteger amount = BigInteger.Parse(tokens[2]);


                if (!dict.ContainsKey(region))
                {
                    dict[region] = new Dictionary<string, BigInteger>();
                    dict[region].Add("Black", 0);
                    dict[region].Add("Green", 0);
                    dict[region].Add("Red", 0);
                    CombinationCheck(dict, region, color, amount);

                }

                dict[region][color] += amount;

                CombinationCheck(dict, region, color, amount);
            }

            foreach (var item in dict.OrderByDescending(x => x.Value["Black"]).ThenBy(x => x.Key.Length).ThenBy(x => x.Key))
            {
                Console.WriteLine(item.Key);
                foreach (var kvp in item.Value.OrderByDescending(x => x.Value).ThenBy(x => x.Key))
                {
                    Console.WriteLine($"-> {kvp.Key} : {kvp.Value}");
                }
            }

        }

        private static void CombinationCheck(Dictionary<string, Dictionary<string, BigInteger>> dict, string region, string color, BigInteger amount)
        {

            if (dict[region]["Green"] >= 0)
            {
                var newReds = (BigInteger)dict[region]["Green"] / 1000000;
                dict[region]["Green"] = dict[region]["Green"] % 1000000;
                dict[region]["Red"] += newReds;
            }

            if (dict[region]["Red"] >= 0)
            {
                var newBlack = (BigInteger)dict[region]["Red"] / 1000000;
                dict[region]["Red"] = dict[region]["Red"] % 1000000;
                dict[region]["Black"] += newBlack;
            }
        }

    }
}
