using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Yelp.Api.Models;

namespace friendbotAPI.Models
{
    public class Message
    {
        public string Body { get; set; }
        public IList<BusinessResponse> Businesses { get; set; }
    }
}