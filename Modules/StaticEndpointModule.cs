using Nancy;

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

            Get("/register", _ => {
                return Response.AsFile("public/register.html");
            });
        }
    }
}