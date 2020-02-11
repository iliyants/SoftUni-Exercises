using Musaca.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Musaca.Services
{
    public interface IUserService
    {
        User CreateUser(string userName,string password, string email);

        bool UsernameAndEmailAreUnique(string username, string email);

        bool SuccesfullLogin(string username, string password);

        User GetUserByUserName(string username);
        User GetUserById(string id);
    }
}
