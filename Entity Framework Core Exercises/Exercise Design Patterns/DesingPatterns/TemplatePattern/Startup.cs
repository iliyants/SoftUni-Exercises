using System;

namespace TemplatePattern
{
    class Startup
    {
        static void Main(string[] args)
        {
            var sourdough = new Sourdough();
            sourdough.Make();

            var twelveGrain = new Sourdough();
            twelveGrain.Make();

            var wholeWheat = new Sourdough();
            wholeWheat.Make();
        }
    }
}
