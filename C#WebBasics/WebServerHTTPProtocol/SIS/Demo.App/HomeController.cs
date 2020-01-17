using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Result;

namespace Demo.App.Controllers
{
    public class HomeController
    {
        public HttpResponse Index(IHttpRequest request)
        {
            string content = "<h1>Hello World!</h1>";


            var htmlResult = new HtmlResult(content, HttpResponseStatusCode.Ok);

            htmlResult.Cookies.AddCookie(new HttpCookie("TEST", "MEST"));

            return htmlResult;
        }
    }
}
