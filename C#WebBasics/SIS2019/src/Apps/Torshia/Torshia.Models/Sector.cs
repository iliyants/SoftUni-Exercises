using System;
using System.Collections.Generic;

namespace Torshia.Models
{
    public class Sector
    {
        public Sector()
        {
            this.Id = Guid.NewGuid().ToString();
            this.SectorTasks = new HashSet<TasksSectors>();
        }
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<TasksSectors> SectorTasks { get; set; }
    }
}
