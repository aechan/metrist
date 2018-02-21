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

            Get("/submitnew", _ => {
                return Response.AsFile("public/submitnew.html");
            });

            Get("/work/:id", args => {
                string id = args.id;
                var res = Response.AsFile("public/getwork.html");
                res.Headers.Add("x-work-id", id);
                return res;
            });
        }
    }
}