using System;
using System.Collections.Generic;

namespace Torshia.Models
{
    public class Task
    {
        public Task()
        {
            this.Id = Guid.NewGuid().ToString();
            this.TaskUsers = new HashSet<UsersTasks>();
            this.AffectedSectors = new HashSet<TasksSectors>();
        }

        public string Title { get; set; }

        public string Id { get; set; }
        public string DueDate { get; set; }
        public bool IsReported { get; set; }
        public string Description { get; set; }
        public ICollection<UsersTasks> TaskUsers { get; set; }

        public ICollection<TasksSectors> AffectedSectors { get; set; }
    }
}
