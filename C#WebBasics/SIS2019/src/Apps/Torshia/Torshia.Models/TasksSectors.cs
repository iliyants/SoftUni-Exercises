using System;
using System.Collections.Generic;
using System.Text;

namespace Torshia.Models
{
    public class TasksSectors
    {
        public string SectorId { get; set; }
        public Sector Sector { get; set; }

        public string TaskId { get; set; }
        public Task Task { get; set; }
    }
}
