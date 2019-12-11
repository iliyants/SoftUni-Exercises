using System;
using System.Collections.Generic;
using System.Linq;

namespace ExamAdvanced
{
    class Startup
    {
        static void Main(string[] args)
        {
            var maxCapacity = int.Parse(Console.ReadLine());
            var input = Console.ReadLine().Split().ToList();

            var stack = new Stack<string>(input);

            var storage = new List<string>();

            while (stack.Any())
            {
                storage.Add(stack.Pop());
            }

            var houseIndexes = new Queue<int>();

            for (int i = 0; i < storage.Count; i++)
            {
                if (char.IsLetter(storage[i][0]))
                {
                    houseIndexes.Enqueue(i);
                }
            }

            var list = new List<int>();

            var usedIndexes = new Queue<int>();

            while (houseIndexes.Any())
            {
                int index = houseIndexes.Dequeue();
                int totalSum = 0;

                for (int i = index; i < storage.Count; i++)
                {
                    var test = storage[i];

                    if (char.IsNumber(test[0]))
                    {
                        if (usedIndexes.Contains(i))
                        {
                            continue;
                        }
                        int number = int.Parse(test);

                        if (number + totalSum <= maxCapacity)
                        {
                            totalSum += number;
                            usedIndexes.Enqueue(i);
                            list.Add(number);
                        }
                        else
                        {
                            Console.WriteLine($"{storage[index]} -> {string.Join(", ", list)}");
                            list.Clear();
                            break;
                        }
                    }
                }

            }

        }
    }
}