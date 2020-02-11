using System;
using System.Collections.Generic;
using System.Text;

namespace SULS.Models
{
    public class Problem
    {
        public Problem()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public int Points { get; set; }

        public int Submissions { get; set; }

    }
}
