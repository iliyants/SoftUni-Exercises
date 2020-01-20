using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRunes.Data;
using IRunes.Models;

namespace IRunes.Services
{
    public class UserService : IUserService
    {
        private RunesDbContext context;

        public UserService()
        {
            this.context = new RunesDbContext();
        }
        public User CreateUser(User user)
        {
            user = this.context.Users.Add(user).Entity;
            this.context.SaveChanges();

            return user;
        }

        public User GetUserByUserNameAndPassword(string username, string password)
        {
            return this.context.Users.SingleOrDefault(user => user.Username == username ||
            user.Email == username && user.Password == password);
        }
    }
}
