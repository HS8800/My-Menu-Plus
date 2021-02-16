using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MyMenuPlus
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //JSON?
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            //Web API Routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
