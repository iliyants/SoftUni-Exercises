using SIS.HTTP.Requests.Contracts;
using System.Collections.Generic;
using System.IO;
using SIS.WebServer.Result;
using SIS.HTTP.Enums;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Xml;
using System.Text;
using SIS.WebServer.Identity;
using SIS.WebServer.ViewEngine;

namespace SIS.WebServer
{
    public class Controller
    {

        private IViewEngine viewEngine = new SISViewEngine();
        public Controller()
        {
            this.ViewData = new Dictionary<string, object>();
        }

        protected Dictionary<string, object> ViewData { get; set; }
        public IHttpRequest Request { get; set; }
        public Principal User =>
            this.Request.Session.ContainsParameter("principal")
            ? (Principal)this.Request.Session.GetParameter("principal")
            : null;
            

        private string ParseTemplate(string viewContent)
        {
            foreach (var param in ViewData)
            {
                viewContent = viewContent.Replace($"@Model.{param.Key}", param.Value.ToString());
            }

            return viewContent;
        }

        protected bool IsLoggedIn()
        {
            return this.Request.Session.ContainsParameter("principal");
        }

        protected void SignIn(string id, string username, string email)
        {
            this.Request.Session.AddParameter("principal", new Principal()
            {
                Id = id,
                Username = username,
                Email = email
            });
        }

        protected void SignOut()
        {
            this.Request.Session.ClearParameters();
        }

        protected ActionResult View([CallerMemberName] string view = null)
        {
            var controllerName = this.GetType().Name.Replace("Controller", "");
            var viewName = view;
            var content = System.IO.File.ReadAllText("Views/" + controllerName + "/" + viewName + ".html");

            string layoutContent = System.IO.File.ReadAllText("Views/_Layout.html");
            layoutContent = ParseTemplate(layoutContent);
            layoutContent = layoutContent.Replace("@RenderBody()", content);

            content = ParseTemplate(layoutContent);

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
