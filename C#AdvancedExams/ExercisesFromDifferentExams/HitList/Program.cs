using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HitList
{
    class Program
    {

        static void Main(string[] args)
        {
            var dict = new Dictionary<string, Dictionary<string, string>>();
            int targetInfoIndex = int.Parse(Console.ReadLine());

            while (true)
            {
                string tokens = Console.ReadLine();
                if (tokens == "end transmissions")
                {
                    break;
                }
                string[] input = tokens.Split(new char[] { ':', '=', ';' }, StringSplitOptions.RemoveEmptyEntries);
                string name = input[0];
                for (int i = 0; i < input.Length - 1; i += 2)
                {
                    string key = input[i + 1];
                    string value = input[i + 2];
                    if (!dict.ContainsKey(name))
                    {
                        dict[name] = new Dictionary<string, string>();
                        dict[name][key] = value;
                    }
                    else
                    {
                        dict[name][key] = value;
                    }
                }
            }


            string[] kill = Console.ReadLine().Split();
            string targetName = kill[1];
            int targetIndex = 0;

            foreach (var item in dict.Where(x => x.Key == targetName))
            {
                foreach (var stats in item.Value)
                {
                    targetIndex += (stats.Key.Length + stats.Value.Length);
                }
            }

            Console.WriteLine($"Info on {targetName}:");

            foreach (var item in dict.Where(x => x.Key == targetName))
            {
                foreach (var info in item.Value.OrderBy(x => x.Key))
                {
                    Console.WriteLine($"---{info.Key}: {info.Value}");
                }
            }
            Console.WriteLine($"Info index: {targetIndex}");

            if (targetIndex >= targetInfoIndex)
            {
                Console.WriteLine("Proceed");
            }
            else
            {
                Console.WriteLine($"Need {targetInfoIndex - targetIndex} more info.");
            }

        }
    }

}