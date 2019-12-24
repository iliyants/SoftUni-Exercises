using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            var kyr = 0;

            for (int i = 0; i < 1000000; i++)
            {
                Task.Run(() =>
                {
                    kyr++;
                });
                Task.Run(() =>
                {
                    kyr++;
                });
            }

            Console.WriteLine(kyr);
        }
    }
}
