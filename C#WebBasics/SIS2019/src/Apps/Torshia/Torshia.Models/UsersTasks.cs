

namespace Torshia.Models
{
    public class UsersTasks
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public string TaskId { get; set; }
        public Task Task { get; set; }
    }
}
