using System;
using System.Collections.Generic;
using System.Text;

namespace VaporStore.DataProcessor.ExportDTOs
{
    public class ExportGenreDTO
    {
        public int Id { get; set; }

        public string Genre { get; set; }

        public ExportGameDTO[] Games { get; set; }

        public int TotalPlayers { get; set; }
    }
}
