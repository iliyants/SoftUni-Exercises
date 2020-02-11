using System;
using System.Collections.Generic;
using System.Text;

namespace Torshia.Services.DTOs
{
    public class TaskDTO
    {
        public int Level { get; set; }

        public string Id { get; set; }
        public string Title { get; set; }
        public List<string> Participants { get; set; }

        public List<string> AffectedSectors { get; set; }

        public string DueDate { get; set; }
        public string Description { get; set; }
    }
}
