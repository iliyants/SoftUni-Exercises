using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NumberOfPrimeNumbers
{
    class Startup
    {
        static void Main(string[] args)
        {
            var timer = new Stopwatch();

            timer.Start();
            //var primeNumbersCount = NumberOfPrimeNumbersSync(2, 1000000);
            //Task.Run() expects a method
            Task.Run(() =>
            {
                var count = NumberOfPrimeNumbersSync(2, 1000000);

                Console.WriteLine($"The count of numbers that are prime in the current range is {count}.");
                timer.Stop();
                Console.WriteLine($"The proccess was completed in {timer.Elapsed}.");

            });

           // timer.Stop();
            //Console.WriteLine($"The count of numbers that are prime in the current range is {primeNumbersCount}.");
            //Console.WriteLine($"The proccess was completed in {timer.Elapsed}.");


            //This is used to demonstrate how while we are using the console,
            //the method continues to work in the back and eventually finishes.
            while (true)
            {
                var inputLine = Console.ReadLine();
                Console.WriteLine(inputLine);
                if (inputLine == "exit")
                {
                    break;
                }
            }
        }

        static int NumberOfPrimeNumbersSync(int from, int to)
        {
            int count = 0;

            for (int i = from; i <= to; i++)
            {
                bool isPrime = true;
                for (int div = 2; div < Math.Sqrt(i); div++)
                {
                    if (i % div == 0)
                    {
                        isPrime = false;
                    }
                }

                if (isPrime)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
