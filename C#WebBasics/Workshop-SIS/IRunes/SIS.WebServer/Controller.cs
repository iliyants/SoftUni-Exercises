using SIS.HTTP.Requests.Contracts;
using System.Collections.Generic;
using SIS.HTTP.Responses.Contracts;
using System.IO;
using SIS.WebServer.Result;
using SIS.HTTP.Enums;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;

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

        protected void SignIn(IHttpRequest httpRequest, string id, string username, string email)
        {
            httpRequest.Session.AddParameter("id", id);
            httpRequest.Session.AddParameter("username", username);
            httpRequest.Session.AddParameter("email", email);
        }

        protected void SignOut(IHttpRequest httpRequest)
        {
            httpRequest.Session.ClearParameters();
        }

        protected ActionResult View([CallerMemberName] string view = null)
        {
            var controllerName = this.GetType().Name.Replace("Controller", "");
            var viewName = view;
            var content = System.IO.File.ReadAllText("Views/" + controllerName + "/" + viewName + ".html");

            content = ParseTemplate(content);

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }

        protected ActionResult Redirect(string location)
        {
            return new RedirectResult(location);
        }

        protected ActionResult Xml(object obj)
        {
            var serializer = new XmlSerializer(obj.GetType());

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), obj, namespaces);

            return new XmlResult(sb.ToString());

        }

        protected ActionResult Json(object obj)
        {
            var serializedObject = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return new JsonResult(serializedObject);
        }

        protected ActionResult File(byte[] fileContent)
        { 
            return new FileResult(fileContent);
        }

        protected ActionResult NotFound(string message)
        {
            return new NotFoundResult(message);
        }
    }
}
