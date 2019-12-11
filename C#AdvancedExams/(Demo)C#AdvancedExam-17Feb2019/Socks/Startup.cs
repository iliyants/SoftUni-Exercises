using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp87
{


    class Startup
    {

        static void Main(string[] args)
        {
            int[] k = Console.ReadLine().Split().Select(int.Parse).ToArray();
            int[] f = Console.ReadLine().Split().Select(int.Parse).ToArray();

            var knives = new Stack<int>(k);
            var forks = new Queue<int>(f);
            var sets = new List<int>();

            while (knives.Any() && forks.Any())
            {
                if (knives.Peek() > forks.Peek())
                {
                    int currentSet = knives.Pop() + forks.Dequeue();
                    sets.Add(currentSet);
                }
                else if (knives.Peek() < forks.Peek())
                {
                    knives.Pop();
                }
                else if (knives.Peek() == forks.Peek())
                {
                    forks.Dequeue();
                    int currentKnife = knives.Pop() + 1;
                    knives.Push(currentKnife);
                }
            }

            Console.WriteLine(sets.Max());
            Console.WriteLine(string.Join(" ", sets));
        }
    }

}