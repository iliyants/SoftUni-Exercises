using Chronometer.Contracts;
using System;
using System.Linq;

namespace Chronometer
{
    class Startup
    {
        static void Main(string[] args)
        {
            IChronometer chronometer = new Chronometer();


            while (true)
            {
                var inputLine = Console.ReadLine();

                switch (inputLine)
                {
                    case "start":chronometer.Start();break;
                    case "stop":chronometer.Stop();break;
                    case "lap": Console.WriteLine(chronometer.Lap());break;
                    case "time": Console.WriteLine(chronometer.GetTime);break;
                    case "laps": Console.WriteLine("Laps: " + (chronometer.Laps.Count == 0 
                        ? "no laps." 
                        : "\r\n" + string.Join("\r\n",chronometer.Laps.Select((lap, index) => $"{index}. {lap}"))));break;
                    case "reset": chronometer.Reset();break;
                    case "exit":return;
                    default:
                        break;
                }
            }
        }
    }
}
