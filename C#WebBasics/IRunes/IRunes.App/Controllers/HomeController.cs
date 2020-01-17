﻿namespace IRunes.App.Controllers
{
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer;
    using SIS.WebServer.Attributes;

    public class HomeController : Controller
    {
        [HttpGet(Url = "/")]
        public IHttpResponse IndexSlash(IHttpRequest httpRequest)
        {
            return Index(httpRequest);
        }
        public IHttpResponse Index(IHttpRequest httpRequest)
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
