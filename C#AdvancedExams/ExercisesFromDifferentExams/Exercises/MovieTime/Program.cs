using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieTime
{

    class MovieArgs
    {
        public MovieArgs(string name, TimeSpan time)
        {
            this.Name = name;
            this.Time = time;
        }

        public string Name { get; set; }
        public TimeSpan Time { get; set; }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var favouriteGenre = Console.ReadLine();
            var favoureteMovieDuration = Console.ReadLine();

            var dict = new Dictionary<string, List<MovieArgs>>();


            while (true)
            {
                var input = Console.ReadLine().Split('|').ToList();
                if (input[0] == "POPCORN!")
                {
                    break;
                }

                string movieName = input[0];
                string movieGenre = input[1];
                var movieTime = TimeSpan.Parse(input[2]);

                if (!dict.ContainsKey(movieGenre))
                {
                    dict[movieGenre] = new List<MovieArgs>();
                }
                if (!dict[movieGenre].Any(x => x.Name.Equals(movieName)))
                {
                    dict[movieGenre].Add(new MovieArgs(movieName, movieTime));
                }

            }

            var sortedList = new List<MovieArgs>();

            foreach (var item in dict.Where(x => x.Key.Equals(favouriteGenre)))
            {
                foreach (var movie in item.Value)
                {
                    sortedList.Add(new MovieArgs(movie.Name, movie.Time));
                }
            }
            switch (favoureteMovieDuration)
            {
                case "Short":
                    sortedList = sortedList.OrderBy(x => x.Time).ThenBy(x => x.Name).ToList();
                    break;
                case "Long":
                    sortedList = sortedList.OrderByDescending(x => x.Time).ThenBy(x => x.Name).ToList();
                    break;
                default:
                    break;
            }

            for (int i = 0; i < sortedList.Count; i++)
            {
                Console.WriteLine(sortedList[i].Name);

                var choice = Console.ReadLine();
                if (choice == "Yes")
                {
                    Console.WriteLine($"We're watching {sortedList[i].Name} - {sortedList[i].Time}");
                    break;
                }
            }

            TimeSpan total = new TimeSpan();
            foreach (var item in dict)
            {
                foreach (var movie in item.Value)
                {
                    total += movie.Time;
                }
            }
            Console.WriteLine($"Total Playlist Duration: {total}");

        }
    }
}