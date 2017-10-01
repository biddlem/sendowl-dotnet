using Newtonsoft.Json;
using System;

namespace SendOwl.Model
{
    public class SendOwlBundle
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Id { get; set; }
        public Components Components { get; set; }
        public string Name { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime Created_at { get; private set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime Updated_at { get; private set; }
        public string Price { get; set; }
        public string Currency_code { get; set; }
        public string Instant_buy_url { get; set; }
        public bool Price_is_minimum { get; set; }
        public string Add_to_cart_url { get; set; }
    }
}
