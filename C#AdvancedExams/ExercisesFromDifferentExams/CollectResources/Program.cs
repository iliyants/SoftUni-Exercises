using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Collect_Resources
{
    class Program
    {
        static void Main(string[] args)
        {


            var input = Console.ReadLine().Split();

            var resources = new string[input.Length];

            string element = string.Empty;
            int quantity = 0;

            for (int i = 0; i < input.Length; i++)
            {
                resources[i] = input[i];
            }

            int numberOfPaths = int.Parse(Console.ReadLine());

            string validResources = "stonegoldwoodfood";

            var collectedResourcesIndexes = new List<int>();

            var allQuantities = new int[numberOfPaths];

            for (int i = 0; i < numberOfPaths; i++)
            {
                var currentPath = Console.ReadLine().Split().Select(int.Parse).ToArray();

                int start = currentPath[0];

                int step = currentPath[1];

                int currentIndexPossition = start;

                while (true)
                {

                    string currentMaterial = resources[currentIndexPossition];

                    Match forElement = Regex.Match(currentMaterial, @"(?<element>[a-z]+)");
                    Match forQuantity = Regex.Match(currentMaterial, @"(?<quantity>[\d]+)");

                    if (forElement.Success)
                    {
                        element = forElement.Groups["element"].Value;
                    }
                    if (forQuantity.Success)
                    {
                        quantity = int.Parse(forQuantity.Groups["quantity"].Value);
                    }
                    else
                    {
                        quantity = 1;
                    }


                    if (validResources.Contains(element) && !collectedResourcesIndexes.Contains(currentIndexPossition))
                    {
                        collectedResourcesIndexes.Add(currentIndexPossition);
                        allQuantities[i] += quantity;
                        currentIndexPossition = (currentIndexPossition + step) % resources.Length;
                    }
                    else if (!validResources.Contains(element))
                    {
                        currentIndexPossition = (currentIndexPossition + step) % resources.Length;
                    }
                    else if (validResources.Contains(element) && collectedResourcesIndexes.Contains(currentIndexPossition))
                    {
                        collectedResourcesIndexes.Clear();
                        break;
                    }

                }
            }

            Console.WriteLine(allQuantities.Max());

        }


    }
}