using System;
using Torshia.Models.Enums;

namespace Torshia.Models
{
    public class Report
    {
        public Report()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public DateTime ReportedOn { get; set; }

        public StatusType Status { get; set; }
        public string TaskId { get; set; }
        public Task Task { get; set; }

        public string UserId { get; set; }
        public User Reporter { get; set; }
    }
}
