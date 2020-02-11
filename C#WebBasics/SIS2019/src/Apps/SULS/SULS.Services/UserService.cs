using SULS.Data;
using SULS.Models;
using SULS.Services.Extensions;
using System.Linq;

namespace SULS.Services
{
    public class UserService : IUserService
    {

        private readonly SULSDbContext context;

        public UserService(SULSDbContext context)
        {
            this.context = context;
        }

        public bool CheckForDuplicateUsers(string username, string email)
        {
            var existingUsername = this.context.Users.Any(x => x.Username == username);
            var existingEmail = this.context.Users.Any(x => x.Email == email);

            return existingUsername || existingEmail;
        }

        public bool CheckForValidUsernameAndPassword(string username, string password)
        {
            var user = this.context.Users.Where(x => x.Username == username).SingleOrDefault();

            if (user != null)
            {
                if (user.Password == password.HashPassword())
                {
                    return true;
                }
            }

            return false;
        }

        public void CreateUser(string username, string email, string password)
        {

            var user = new User()
            {
                Username = username,
                Email = email,
                Password = password.HashPassword()
            };

            this.context.Users.Add(user);
            this.context.SaveChanges();
        }

        public User GetUserByUsername(string username)
        {
            return this.context.Users.Where(x => x.Username == username).SingleOrDefault();
        }
    }
}
