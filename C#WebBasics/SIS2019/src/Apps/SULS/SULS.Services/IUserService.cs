using SULS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SULS.Services
{
    public interface  IUserService
    {
        void CreateUser(string username, string email,string password);

        bool CheckForDuplicateUsers(string username, string email);

        bool CheckForValidUsernameAndPassword(string username, string password);

        User GetUserByUsername(string username);
    }
}
