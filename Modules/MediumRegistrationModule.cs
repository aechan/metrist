using Medium;
using Nancy;
using Nancy.Extensions;
using Nancy.IO;
using Newtonsoft.Json;

namespace Metrist.Modules
{
    public class MediumRegistrationModule : NancyModule
    {
        public MediumRegistrationModule()
        {
            Get("/api/getMediumAuthURL", args => {
                var oAuthClient = new Medium.OAuthClient("c582c34530cc", "03f8d756aee03697bbc58487eca1f8caf57d1736");
                var url = oAuthClient.GetAuthorizeUrl(
                    "secretstate",
                    "http://127.0.0.1:5000/register",
                    new[]
                    {
                        Medium.Authentication.Scope.BasicProfile,
                        Medium.Authentication.Scope.ListPublications
                    });
                return url;
            });
        }
    }
}