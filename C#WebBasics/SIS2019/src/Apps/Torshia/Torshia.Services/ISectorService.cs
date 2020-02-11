
using System.Collections.Generic;
using Torshia.Models;

namespace Torshia.Services
{
    public interface ISectorService
    {
        void CreateSectorsIfTheyDoesntExist(List<string> sectors);

        Sector GetSectorByName(string sectorName);
    }
}
