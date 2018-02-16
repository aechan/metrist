using Nancy;
using Nancy.Extensions;
using Nancy.IO;
using Medium;
using System.Linq;
using FrostSharp;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Metrist.Modules
{
    public class PoetMediumConnectorModule : NancyModule
    {
        private const bool FrostLogging = true;
        public PoetMediumConnectorModule()
        {
            Post("/api/postWork", args => {
                var body = RequestStream.FromStream(Request.Body).AsString();

                if(!string.IsNullOrWhiteSpace(body))
                {
                    MediumToPoet(body, Request.Headers["x-frost-api"].FirstOrDefault(), Request.Headers["x-medium-api"].FirstOrDefault());
                    return "OK";
                }
                else
                {
                    var res = new Response();
                    res.StatusCode = HttpStatusCode.BadRequest;
                    res.ReasonPhrase = "Empty body sent";
                    return res;
                }
            });

            Get("/api/getAllWorks", args => {
                var frost = new Frost(Request.Headers["x-frost-api"].FirstOrDefault(), new Configuration(), FrostLogging);
                var works = frost.GetAllWorks().Result;

                string resp = "{\"works\": [";
                for(int i = 0; i < works.Count-1; i++)
                {
                    resp+=works[i].ToJSON()+",";
                }
                resp+=works[works.Count-1].ToJSON();
                resp += "]}";

                return resp;
            });
        }

        private void MediumToPoet(string url, string frostAPI, string mediumAPI)
        {
            var oAuthClient = new Medium.OAuthClient("c582c34530cc", "03f8d756aee03697bbc58487eca1f8caf57d1736");
            var accessToken = oAuthClient.GetAccessToken(mediumAPI, "http://metrist.org/register");

            var client = new Medium.Client();
            var user = client.GetCurrentUser(accessToken);

            var pubs = client.GetPublications(user.Id, accessToken);

            var theWork = pubs.Where(x => x.Url == url).FirstOrDefault();

            var frost = new Frost(frostAPI, new Configuration(), FrostLogging);
            frost.CreateWork(new WorkAttributes(
                theWork.Name,
                DateTime.UtcNow,
                DateTime.UtcNow,
                user.Name,
                theWork.Description + " Read full text at: " + theWork.Url
            ));
        }
    }
}