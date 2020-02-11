using System;
using System.Collections.Generic;
using System.Text;
using Torshia.Models;

namespace Torshia.Services
{
    public interface IUserService
    {
        void CreateUser(string username, string email, string password);

        bool CheckIfUserExists(string username, string password);

        bool CheckForDuplicateUsernameOrEmail(string username, string email);

        User GetUserByUserName(string username);

        bool ChecksForExistingUsersByUsernames(string usernames);

        List<User> ReturnUsersByUsernames(string usernames);
    }
}
