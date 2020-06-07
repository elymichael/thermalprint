

namespace thermalprinting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Swashbuckle.Application;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services            
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            // Web API routes
            config.MapHttpAttributeRoutes();            

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config
               .EnableSwagger(c => c
                   .SingleApiVersion("v1", "ThermalPrinting.API application")
                   .Description("ThermalPrinting.API is an REST API developed by Sitcs")
                   .TermsOfService("Only use if you are an official collaborate of Sitcs")
                   .Contact(cc => cc
                       .Email("elymichael@sitcsrd.com")
                       .Name("Ely Michael Núñez")
                       .Url("http://www.sitcsrd.com")))
               .EnableSwaggerUi();
        }
    }
}
