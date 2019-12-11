using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

class SportCards
{
    class TagLikes
    {
        public TagLikes(string tag, int likes)
        {
            this.Tag = tag;
            this.Likes = likes;
        }
        public string Tag { get; set; }
        public int Likes { get; set; }
    }

    class User
    {
        public User(string name, int totalLikes, int totalTags)
        {
            this.Name = name;
            this.TotalLikes = totalLikes;
            this.TotalTags = totalTags;
        }
        public string Name { get; set; }
        public int TotalLikes { get; set; }
        public int TotalTags { get; set; }
    }

    static void Main()
    {

        var dict = new Dictionary<User, List<TagLikes>>();

        while (true)
        {
            string input = Console.ReadLine();
            if (input == "end")
            {
                break;
            }

            string[] tokens = input.Split(" -> ");

            if (tokens.Length == 3)
            {
                string userName = tokens[0];
                string tag = tokens[1];
                int likes = int.Parse(tokens[2]);

                if (!dict.Any(x => x.Key.Name == userName))
                {
                    User newUser = new User(userName, 0, 0);
                    TagLikes newTagLikes = new TagLikes(tag, likes);
                    dict[newUser] = new List<TagLikes>();
                    dict[newUser].Add(newTagLikes);
                }
                else
                {
                    var currentUserIndex = dict.Keys.First(x => x.Name == userName);
                    if (dict[currentUserIndex].Any(x => x.Tag == tag))
                    {
                        int tagIndex = dict[currentUserIndex].FindIndex(x => x.Tag == tag);
                        dict[currentUserIndex][tagIndex].Likes += likes;
                    }
                    else
                    {
                        dict[currentUserIndex].Add(new TagLikes(tag, likes));

                    }
                }

            }
            else
            {
                string[] delete = input.Split();
                if (dict.Any(x => x.Key.Name == delete[1]))
                {
                    var userToDeleteIndex = dict.Keys.First(x => x.Name == delete[1]);
                    dict.Remove(userToDeleteIndex);
                }

            }
        }


        foreach (var item in dict)
        {
            foreach (var kvp in item.Value)
            {
                item.Key.TotalLikes += kvp.Likes;
                item.Key.TotalTags++;
            }
        }

        foreach (var item in dict.OrderByDescending(x => x.Key.TotalLikes).ThenBy(x => x.Key.TotalTags))
        {
            Console.WriteLine(item.Key.Name);
            foreach (var kvp in item.Value)
            {
                Console.WriteLine($"- {kvp.Tag}: {kvp.Likes}");
            }
        }


    }
}
