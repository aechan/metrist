using Nancy;
using Nancy.Extensions;
using Nancy.IO;
using System.Linq;
using FrostSharp;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using CodeHollow.FeedReader;

namespace Metrist.Modules
{
    public class PoetMediumConnectorModule : NancyModule
    {
        private const bool FrostLogging = false;
        public PoetMediumConnectorModule()
        {
            Post("/api/postWork", args => {
                var body = RequestStream.FromStream(Request.Body).AsString();
                if(!string.IsNullOrWhiteSpace(body))
                {
                    string poet = Request.Headers["x-frost-api"].FirstOrDefault();
                    string medium = Request.Headers["x-medium-api"].FirstOrDefault();                    

                    MediumToPoet(body, poet, medium);
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
                for(int i = works.Count-1; i > 0; i--)
                {
                    resp+=works[i].ToJSON()+",";
                }
                resp+=works[0].ToJSON();
                resp += "]}";

                return resp;
            });

            Post("/api/postRawWork", args => {
                var body = RequestStream.FromStream(Request.Body).AsString();
                
                if(!string.IsNullOrWhiteSpace(body))
                {
                    string poet = Request.Headers["x-frost-api"].FirstOrDefault();

                    var work = JsonConvert.DeserializeObject<WorkAttributes>(body);

                    var frost = new Frost(poet, new Configuration(), FrostLogging);
                    frost.CreateWork(work);
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
        }

        private void MediumToPoet(string url, string frostAPI, string mediumUsername)
        {
            var feed = FeedReader.ReadAsync("https://medium.com/feed/"+mediumUsername).Result;

            var work = feed.Items.Where(x => x.Link.Split('?')[0] == url).FirstOrDefault();
            var specItem = (CodeHollow.FeedReader.Feeds.Rss20FeedItem)work.SpecificItem;

            var frost = new Frost(frostAPI, new Configuration(), FrostLogging);

            var date = work.PublishingDate == null ? work.PublishingDate.Value : DateTime.UtcNow;
            
            var author = specItem.DC.Creator;

            var id = frost.CreateWork(new WorkAttributes(
                work.Title,
                date,
                date,
                author,
                work.Content + "<br/>Posted from <a href='http://metrist.org'>metrist</a>. <a href='"+url+"'>Source</a>"
            )).Result;
        }
    }
}