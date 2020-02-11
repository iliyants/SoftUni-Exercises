using System;
using System.Collections.Generic;
using System.Linq;
using Torshia.Data;
using Torshia.Models;
using Torshia.Services.Extensions;

namespace Torshia.Services
{
    public class UserService : IUserService
    {

        private readonly ToshiaDbContext context;

        public UserService(ToshiaDbContext context)
        {
            this.context = context;
        }

        public bool CheckForDuplicateUsernameOrEmail(string username, string email)
        {
            var usernameExists = this.context.Users.Any(x => x.Username == username);
            var emailExists = this.context.Users.Any(x => x.Email == email);

            if (usernameExists || emailExists)
            {
                return true;
            }

            return false;
        }

        public void CreateUser(string username, string email, string password)
        {
            var user = new User()
            {
                Username = username,
                Email = email,
                Password = password.HashPassword(),
                Role = Models.Enums.RoleType.Admin
            };

            this.context.Users.Add(user);
            context.SaveChanges();
        }

        public bool CheckIfUserExists(string username, string password)
        {
            var user = this.context.Users.SingleOrDefault(x => x.Username == username);

            if (user != null)
            {
                if (user.Password == password.HashPassword())
                {
                    return true;
                }
            }

            return false;
        }

        public User GetUserByUserName(string username)
        {
            return this.context.Users.SingleOrDefault(x => x.Username == username);
        }

        public bool ChecksForExistingUsersByUsernames(string usernames)
        {
            var listOfUsernames = usernames.Split(new char[] { ' ', '\n', '\t', ',' },
                StringSplitOptions.RemoveEmptyEntries).ToHashSet();

            var listOfDbUserNames = this.context.Users.Select(x => x.Username).ToHashSet();

            return new HashSet<string>(listOfDbUserNames).IsSupersetOf(listOfUsernames);
 
        }

        public List<User> ReturnUsersByUsernames(string usernames)
        {
            var listOfUsernames = usernames.Split(new char[] { ' ', '\n', '\t', ',' },
                StringSplitOptions.RemoveEmptyEntries).ToHashSet();

            var result = new List<User>();

            foreach (var username in listOfUsernames)
            {
                result.Add(GetUserByUserName(username));
            }

            return result;
        }
    }
}
