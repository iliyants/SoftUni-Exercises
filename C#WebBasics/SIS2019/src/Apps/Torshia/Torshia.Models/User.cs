using System;
using System.Collections.Generic;
using Torshia.Models.Enums;

namespace Torshia.Models
{
    public class User
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.UserTasks = new HashSet<UsersTasks>();
        }
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public RoleType Role { get; set; }

        public ICollection<UsersTasks> UserTasks { get; set; }

    }
}
