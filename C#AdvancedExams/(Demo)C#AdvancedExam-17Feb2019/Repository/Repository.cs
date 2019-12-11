using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class Repository
    {
        public List<Person> Data;

        public Repository()
        {
            this.Data = new List<Person>();
        }


        public void Add(Person person)
        {
            this.Data.Add(person);
        }

        public Person Get(int id)
        {
            var person = this.Data[id];
            return person;
        }

        public bool Update(int id, Person replacement)
        {
            if (id > this.Data.Count - 1)
            {
                return false;
            }
            else
            {
                this.Data[id] = replacement;
                return true;
            }
        }

        public bool Delete(int id)
        {
            if (id > this.Data.Count - 1)
            {
                return false;
            }
            else
            {
                this.Data.RemoveAt(id);
                return true;
            }
        }

        public int Count => this.Data.Count;       
       
    }
}
