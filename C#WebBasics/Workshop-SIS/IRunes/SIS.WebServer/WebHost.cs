using SIS.HTTP.Enums;
using SIS.HTTP.Responses;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Attributes;
using SIS.WebServer.Result;
using SIS.WebServer.Routing;
using SIS.WebServer.Routing.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SIS.WebServer
{
    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            AutoRegisterRoutes(application, serverRoutingTable);
            application.ConfigureServices();
            application.Configure(serverRoutingTable);

            Server server = new Server(8000, serverRoutingTable);
            server.Run();
        }

        private static void AutoRegisterRoutes(IMvcApplication application, IServerRoutingTable serverRoutingTable)
        {
            var controllers = application
                .GetType()
                .Assembly
                .GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract
                 && type.IsSubclassOf(typeof(Controller)));


            foreach (var controller in controllers)
            {
                var actions = controller
                    .GetMethods(
                            BindingFlags.DeclaredOnly
                          | BindingFlags.Public
                          | BindingFlags.Instance)
                    .Where(f => !f.IsSpecialName && !IsOverride(f)
                      && f.GetCustomAttributes(typeof(NonActionAttribute), false).Length == 0);

                foreach (var action in actions)
                {
                    var attribute =
                        action.GetCustomAttributes()
                        .Where(x => x.GetType()
                        .IsSubclassOf(typeof(BaseHttpAttribute)))
                        .LastOrDefault() as BaseHttpAttribute;
                        

                    var path = $"/{controller.Name.Replace("Controller", string.Empty)}/{action.Name}";

                    var httpMethod = HttpRequestMethod.Get;


                    if (attribute != null)
                    {
                        httpMethod = attribute.Method;
                    }

                    if (attribute?.Url != null)
                    {
                        path = attribute.Url;
                    }

                    if (attribute?.ActionName != null)
                    {
                        path = $"/{controller.Name.Replace("Controller", string.Empty)}/{attribute.ActionName}";
                    }

                    serverRoutingTable.Add(httpMethod, path, request =>
                    {
                        var controllerInstance = Activator.CreateInstance(controller);
                        ((Controller)controllerInstance).Request = request;

                        var principal = ((Controller)controllerInstance).User;
                        var authorizeAttribute = action.GetCustomAttributes()
                        .LastOrDefault(a => a.GetType() == typeof(AuthorizeAttribute)) as AuthorizeAttribute;

                        if (authorizeAttribute != null && !authorizeAttribute.IsInAuthority(principal))
                        {
                            return new HttpResponse(HttpResponseStatusCode.Forbidden);
                        }
                        var response = action.Invoke(controllerInstance, new object[0]) as ActionResult;
                        return response;
                    });

                    Console.WriteLine(httpMethod + " " + path);
                }
            }          
        }

        public static bool IsOverride(MethodInfo m)
        {
            return m.GetBaseDefinition().DeclaringType != m.DeclaringType;
        }

    }
}
