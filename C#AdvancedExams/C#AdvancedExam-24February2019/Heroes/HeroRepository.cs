using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Heroes
{
    public class HeroRepository
    {
        private List<Hero> data;


        public HeroRepository()
        {
            this.data = new List<Hero>();
        }


        public void Add(Hero hero)
        {
            this.data.Add(hero);   
        }

        public void Remove(string name)
        {
            var elementToRemove = this.data.FindIndex(x => x.Name.Equals(name));
            this.data.RemoveAt(elementToRemove);
        }

        public Hero GetHeroWithHighestStrength()
        {
            var ordered = this.data.OrderByDescending(x => x.Item.Strength).ToList();
            return this.data[0];
        }

        public Hero GetHeroWithHighestAbility()
        {
            var ordered = this.data.OrderByDescending(x => x.Item.Ability).ToList();
            return this.data[0];
        }

        public Hero GetHeroWithHighestIntelligence()
        {
            var ordered = this.data.OrderByDescending(x => x.Item.Intelligence).ToList();
            return this.data[0];
        }

        public int Count => this.data.Count;


        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var item in this.data)
            {
                sb.Append(item.ToString());
            }

            return sb.ToString();
        }


    }
}
