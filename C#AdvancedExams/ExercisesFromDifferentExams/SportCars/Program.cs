using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    class Sport
    {
        public Sport(string sportName, decimal price)
        {
            this.SportName = sportName;
            this.Price = price;
        }
        public string SportName { get; set; }
        public decimal Price { get; set; }
    }

    static void Main()
    {

        var dict = new Dictionary<string, List<Sport>>();

        while (true)
        {
            string input = Console.ReadLine();
            if (input == "end")
            {
                break;
            }

            string[] tokens = input.Split(" - ");

            if (tokens.Length == 3)
            {
                string card = tokens[0];
                string sport = tokens[1];
                decimal price = decimal.Parse(tokens[2]);

                if (!dict.ContainsKey(card))
                {
                    Sport newCard = new Sport(sport, price);
                    dict[card] = new List<Sport>();
                    dict[card].Add(newCard);
                }
                else
                {
                    if (dict[card].Any(x => x.SportName == sport))
                    {
                        var index = dict[card].FindIndex(x => x.SportName == sport);
                        dict[card][index].Price = price;
                    }
                    else
                    {
                        dict[card].Add(new Sport(sport, price));
                    }
                }
            }
            else
            {
                string[] checking = input.Split();
                if (dict.ContainsKey(checking[1]))
                {
                    foreach (var item in dict.Where(x => x.Key == checking[1]))
                    {
                        Console.WriteLine($"{item.Key} is available!");
                        break;
                    }
                }
                else
                {
                    Console.WriteLine($"{checking[1]} is not available!");
                }
            }
        }

        foreach (var item in dict.OrderByDescending(x => x.Value.Count))
        {
            Console.WriteLine($"{item.Key}:");
            foreach (var kvp in item.Value.OrderBy(x => x.SportName))
            {
                Console.WriteLine($"  -{kvp.SportName} - {kvp.Price:f2}");
            }
        }


    }
}