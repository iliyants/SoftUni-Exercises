using System;
using System.Collections.Generic;
using System.Text;

namespace TemplatePattern
{
    public class WholeWheat:Bread
    {
        public override void Bake()
        {
            Console.WriteLine("Gathering Ingredoents for WholeWheat Bread.");
        }

        public override void MixIngredients()
        {
            Console.WriteLine("Baking the WholeWheat Bread. (15 minutes)");
        }
    }
}
