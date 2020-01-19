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
        public ActionResult IndexSlash(IHttpRequest httpRequest)
        {
            return Index(httpRequest);
        }
        public ActionResult Index(IHttpRequest httpRequest)
        {
            if (this.IsLoggedIn(httpRequest))
            {
                this.ViewData.Add("Username", httpRequest.Session.GetParameter("username").ToString());
                return this.View("/Index-Logged");
            }

            return this.View();
        }


    }
}
