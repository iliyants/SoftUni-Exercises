﻿namespace IRunes.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using IRunes.Data;
    using IRunes.Models;
    using SIS.HTTP.Requests.Contracts;
    using SIS.WebServer;
    using SIS.WebServer.Attributes;
    using SIS.WebServer.Result;

    public class UsersController : Controller
    {
        public ActionResult Register(IHttpRequest httpRequest)
        {
            return this.View();
        }

        [HttpPost(ActionName = "Register")]
        public ActionResult RegisterConfirm(IHttpRequest httpRequest)
        {
            using (var context = new RunesDbContext())
            {
                var username = ((ISet<string>)httpRequest.FormData["username"]).FirstOrDefault();
                var password = ((ISet<string>)httpRequest.FormData["password"]).FirstOrDefault();
                var confirmPassword = ((ISet<string>)httpRequest.FormData["confirmPassword"]).FirstOrDefault();
                var email = ((ISet<string>)httpRequest.FormData["email"]).FirstOrDefault();

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

           

                context.Users.Add(user);
                context.SaveChanges();
            }

            return this.Redirect("/Users/Login");
        }

        public ActionResult Login(IHttpRequest httpRequest)
        {
            return this.View();
        }

        [HttpPost(ActionName = "Login")]
        public ActionResult LoginConfirm(IHttpRequest httpRequest)
        {
            using (var context = new RunesDbContext())
            {
                var username = ((ISet<string>)httpRequest.FormData["username"]).FirstOrDefault();
                var password = ((ISet<string>)httpRequest.FormData["password"]).FirstOrDefault();
                var hashedPassword = this.HashPassword(password);

                var userFromDb = context.Users
                    .FirstOrDefault(u => (u.Username == username || u.Email == username) && u.Password == hashedPassword);

                if (userFromDb == null)
                {
                    return this.Redirect("/Users/Login");
                }

                this.SignIn(httpRequest, userFromDb.Id,userFromDb.Username,userFromDb.Email);
            }

            return this.Redirect("/");
        }

        public ActionResult Logout(IHttpRequest httpRequest)
        {
            this.SignOut(httpRequest);
            return this.Redirect("/");
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }
    }
}
