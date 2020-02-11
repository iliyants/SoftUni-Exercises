using Musaca.Data;
using Musaca.Models;
using Musaca.Services.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Musaca.Services
{
    public class UserService : IUserService
    {

        private readonly MusacaDbContext context;

        public UserService(MusacaDbContext context)
        {
            this.context = context;
        }

        public User CreateUser(string userName, string password, string email)
        {
            var user = new User()
            {
                Username = userName,
                Password = password.HashPassword(),
                Email = email
            };

            var result = this.context.Add(user).Entity;
            context.SaveChanges();

            return result;
        }

        public User GetUserById(string id)
        {
            return this.context.Users.SingleOrDefault(x => x.Id == id);

        }

        public User GetUserByUserName(string username)
        {
            return this.context.Users.SingleOrDefault(x => x.Username == username);
        }

        public bool SuccesfullLogin(string username, string password)
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

        public bool UsernameAndEmailAreUnique(string username, string email)
        {
            var usernameEmailHolder = this.context.Users.ToDictionary(x => x.Username, x => x.Email);

            return !usernameEmailHolder.Keys.Contains(username) && !usernameEmailHolder.Values.Contains(email);
        }
    }
}
