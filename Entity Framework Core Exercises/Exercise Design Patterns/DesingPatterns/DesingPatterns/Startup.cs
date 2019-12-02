using PrototypeDesingPattern;
using System;

namespace DesingPatterns
{
    class Startup
    {
        static void Main(string[] args)
        {
            var sandwichMenu = new SandwichMenu();

            sandwichMenu["BLT"] = new Sandwich("White", "Bacon", "", "Lettuce, Tomato");
            sandwichMenu["PB&J"] = new Sandwich("White", "", "", "Peanut Butter, Jelly");
            sandwichMenu["Turkey"] = new Sandwich("Rye", "Turkey", "Swiss", "Lettuce, Onion, Tomato");

            sandwichMenu["LoadedBLT"] = new Sandwich("White", "Turkey, Bacon", "American", "Lettuce, Tomato, Onion, Olives");
            sandwichMenu["ThreeMeatCombo"] = new Sandwich("Rye", "Turkey, Ham, Salami", "Provolone", "Lettuce, Onion");
            sandwichMenu["VeganGlutenFree"] = new Sandwich("", "", "", "");

            var firstSandwich = (Sandwich)sandwichMenu["BLT"].Clone();


        }
    }
}
