using IRunes.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Services
{
    public interface IUserService
    {
        User CreateUser(User user);

        User GetUserByUserNameAndPassword(string username, string password);

    }
}
