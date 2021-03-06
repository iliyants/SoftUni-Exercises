﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PrototypeDesingPattern
{
    public class Sandwich : SandwichPrototype
    {
        private string bread;
        private string meat;
        private string cheese;
        private string vegies;

        public Sandwich(string bread, string meat,string cheese,string vegies)
        {
            this.bread = bread;
            this.meat = meat;
            this.cheese = cheese;
            this.vegies = vegies;
        }
        public override SandwichPrototype Clone()
        {
            var ingredients = this.GetIngredientsList();
            Console.WriteLine($"Cloning sandwich with ingredients: {ingredients}");

            return (SandwichPrototype)this.MemberwiseClone();
        }

        private string GetIngredientsList()
        {
            return $"{this.bread}, {this.meat}, {this.cheese}, {this.vegies}";
        }
    }
}
