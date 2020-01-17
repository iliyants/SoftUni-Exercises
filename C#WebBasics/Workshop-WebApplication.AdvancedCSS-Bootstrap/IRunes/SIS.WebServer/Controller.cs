﻿using SIS.HTTP.Requests.Contracts;
using System.Collections.Generic;
using SIS.HTTP.Responses.Contracts;
using System.IO;
using SIS.WebServer.Result;
using SIS.HTTP.Enums;
using System.Runtime.CompilerServices;

namespace SIS.WebServer
{
    public class Controller
    {
        public Controller()
        {
            this.ViewData = new Dictionary<string, object>();
        }

        protected Dictionary<string, object> ViewData { get; set; }

        private string ParseTemplate(string viewContent)
        {
            foreach (var param in ViewData)
            {
                viewContent = viewContent.Replace($"@Model.{param.Key}", param.Value.ToString());
            }

            return viewContent;
        }

        protected bool IsLoggedIn(IHttpRequest httpRequest)
        {
            return httpRequest.Session.ContainsParameter("username");
        }

        protected void SignIn(IHttpRequest httpRequest, string id, string username, string email )
        {
            httpRequest.Session.AddParameter("id", id);
            httpRequest.Session.AddParameter("username", username);
            httpRequest.Session.AddParameter("email", email);
        }

        protected void SignOut(IHttpRequest httpRequest)
        {
            httpRequest.Session.ClearParameters();
        }

        protected IHttpResponse View([CallerMemberName] string view = null)
        {
            var controllerName = this.GetType().Name.Replace("Controller", "");
            var viewName = view;
            var content = File.ReadAllText("Views/" + controllerName + "/" + viewName + ".html");

            content = ParseTemplate(content);

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }

        protected IHttpResponse Redirect(string location)
        {
            return new RedirectResult(location);
        }


    }
}
