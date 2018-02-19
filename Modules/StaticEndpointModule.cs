using Nancy;
using System;

namespace Metrist.Modules
{
    public class StaticEndpointModule : NancyModule
    {
        public StaticEndpointModule()
        {
            Get("/", _ => {
               return Response.AsFile("public/index.html"); 
            });

            Get("/submit", _ => {
                return Response.AsFile("public/submit.html");
            });
        }
    }
}