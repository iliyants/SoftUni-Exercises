using System;
using System.Collections.Generic;
using System.Linq;

namespace PopulationCounter
{

    class Country
    {
        public Country(string name)
        {
            this.Name = name;
            this.Population = 0;
        }

        public string Name { get; set; }
        public long Population { get; set; }
    }

    class Cities
    {
        public Cities(string name, long population)
        {
            this.Name = name;
            this.Population = population;
        }

        public string Name { get; set; }
        public long Population { get; set; }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var dict = new Dictionary<Country, List<Cities>>();
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "report")
                {
                    break;
                }

                var split = input.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToArray();

                string city = split[0];
                string country = split[1];
                long population = long.Parse(split[2]);

                if (!dict.Keys.Any(x => x.Name == country))
                {
                    var newCountry = new Country(country);
                    var newCity = new Cities(city, population);
                    dict.Add(newCountry, new List<Cities>());
                    dict[newCountry].Add(newCity);
                }
                else
                {
                    var currentCountry = dict.Keys.First(x => x.Name == country);
                    dict[currentCountry].Add(new Cities(city, population));
                }
            }

            foreach (var country in dict)
            {
                long totalPopulation = 0;
                foreach (var kvp in country.Value)
                {
                    totalPopulation += kvp.Population;
                }
                country.Key.Population = totalPopulation;
            }

            foreach (var country in dict.OrderByDescending(x => x.Key.Population))
            {
                Console.WriteLine($"{country.Key.Name} (total population: {country.Key.Population})");
                foreach (var city in country.Value.OrderByDescending(x => x.Population))
                {
                    Console.WriteLine($"=>{city.Name}: {city.Population}");
                }
            }
        }
    }
}