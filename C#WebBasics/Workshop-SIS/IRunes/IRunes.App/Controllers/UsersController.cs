namespace IRunes.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using IRunes.Data;
    using IRunes.Models;
    using IRunes.Services;
    using SIS.WebServer;
    using SIS.WebServer.Attributes;
    using SIS.WebServer.Result;

    public class UsersController : Controller
    {
        private IUserService userService;
        public UsersController()
        {
            this.userService = new UserService();
        }
        public ActionResult Register()
        {
            return this.View();
        }

        [HttpPost(ActionName = "Register")]
        public ActionResult RegisterConfirm()
        {

            var username = ((ISet<string>)this.Request.FormData["username"]).FirstOrDefault();
            var password = ((ISet<string>)this.Request.FormData["password"]).FirstOrDefault();
            var confirmPassword = ((ISet<string>)this.Request.FormData["confirmPassword"]).FirstOrDefault();
            var email = ((ISet<string>)this.Request.FormData["email"]).FirstOrDefault();

            if (password != confirmPassword)
            {
                return this.Redirect("/Users/Register");
            }

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = username,
                Password = this.HashPassword(password),
                Email = email
            };

            this.userService.CreateUser(user);

            return this.Redirect("/Users/Login");
        }

        public ActionResult Login()
        {
            return this.View();
        }

        [HttpPost(ActionName = "Login")]
        public ActionResult LoginConfirm()
        {

            var username = ((ISet<string>)this.Request.FormData["username"]).FirstOrDefault();
            var password = ((ISet<string>)this.Request.FormData["password"]).FirstOrDefault();
            var hashedPassword = this.HashPassword(password);

            var userFromDb = this.userService.GetUserByUserNameAndPassword(username, hashedPassword);

            if (userFromDb == null)
            {
                return this.Redirect("/Users/Login");
            }

            this.SignIn(userFromDb.Id, userFromDb.Username, userFromDb.Email);

            return this.Redirect("/");
        }

        public ActionResult Logout()
        {
            this.SignOut();
            return this.Redirect("/");
        }

        [NonAction]
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }
    }
}
