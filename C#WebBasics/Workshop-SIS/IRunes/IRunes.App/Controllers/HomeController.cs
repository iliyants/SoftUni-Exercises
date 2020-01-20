namespace IRunes.App.Controllers
{
    using IRunes.Models;
    using SIS.HTTP.Requests.Contracts;
    using SIS.WebServer;
    using SIS.WebServer.Attributes;
    using SIS.WebServer.Result;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public class HomeController : Controller
    {
        [HttpGet(Url = "/")]
        public ActionResult IndexSlash()
        {
            return Index();
        }

        public ActionResult Index()
        {
            if (this.IsLoggedIn())
            {
                this.ViewData["Username"] = this.User.Username;
                return this.View("/Index-Logged");
            }

            return this.View();
        }


    }
}
