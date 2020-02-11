using System.Collections.Generic;
using System.Linq;
using Torshia.Data;
using Torshia.Models;

namespace Torshia.Services
{
    public class SectorService : ISectorService
    {

        private readonly ToshiaDbContext context;

        public SectorService(ToshiaDbContext context)
        {
            this.context = context;
        }

        public void CreateSectorsIfTheyDoesntExist(List<string> sectorNames)
        {

            foreach (var name in sectorNames)
            {
                if (!this.context.Sectors.Any(x => x.Name == name))
                {
                    var sector = new Sector()
                    {
                        Name = name
                    };

                    this.context.Sectors.Add(sector);
                    this.context.SaveChanges();
                }
            }

        }

        public Sector GetSectorByName(string sectorName)
        {
            return this.context.Sectors.SingleOrDefault(x => x.Name == sectorName);
        }
    }
}
