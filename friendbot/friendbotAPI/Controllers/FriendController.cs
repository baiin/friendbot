using System;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

using ApiAiSDK;
using ApiAiSDK.Model;

using Yelp.Api;
using Yelp.Api.Models;

using friendbotAPI.Models;

namespace friendbotAPI.Controllers
{
    public class FriendController : ApiController
    {
        private ApiAi apiAi;

        [HttpPost]
        [Route("api/friend")]
        public async Task<Message> RetrieveIntent(Query inboundQuery) 
        {
            var config = new AIConfiguration(ConfigurationManager.AppSettings["APIAI_TOKEN"], SupportedLanguage.English);
            
            apiAi = new ApiAi(config);

            var response = apiAi.TextRequest(inboundQuery.Body);

            var intent = response.Result.Metadata.IntentName;

            if (intent == null)
            {
                return new Message()
                {
                    Body = response.Result.Fulfillment.Speech
                };
            }
            else
            {
                var city = response.Result.Parameters.Where(p => p.Key == "geo-city").FirstOrDefault().Value.ToString();
                var state = response.Result.Parameters.Where(p => p.Key == "geo-state-us").FirstOrDefault().Value.ToString();

                if (city == "")
                {
                    return new Message()
                    {
                        Body = "Please try again with a particular city."
                    };
                }
                else
                {
                    var client = new Yelp.Api.Client(ConfigurationManager.AppSettings["YELPAPI_ID"], ConfigurationManager.AppSettings["YELPAPI_TOKEN"]);

                    var request = new Yelp.Api.Models.SearchRequest();
                    request.Term = intent;
                    request.Location = city;
                    request.MaxResults = 5;
                    request.OpenNow = true;

                    var results = await client.SearchBusinessesAllAsync(request);

                    return new Message()
                    {
                        Body = intent + " in " + city + " " + state,
                        Businesses = results.Businesses
                    };
                }
            }
        }
    }
}
