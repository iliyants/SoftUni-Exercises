using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParallelMergeSort
{
    class Program
    {
        static void Main(string[] args)
        {
            var array = File
                 .ReadAllText("./TestData.txt")
                 .Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                 .Select(element => int.Parse(element))
                 .ToArray();


            Task sort = MergeSort.Sort(array, 0, array.Length - 1);

            Console.WriteLine(string.Join(Environment.NewLine, array));


        }

    }
}
