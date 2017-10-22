using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SendOwl.Model
{
    public class SendOwlDiscount : SendOwlObject<int>
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public override int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Expires_at { get; set; }
        public int? Usage_limit { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime Created_at { get; private set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime Updated_at { get; private set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Code { get; set; }
        /// <summary>
        /// Can be set when using "Use_limited_type.Many_codes_one_use". Omit if generating codes
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<string> Codes { get; set; }
        public string Minimum_cart_value { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public LimitingType Use_limited_type { get; set; }
        /// <summary>
        /// Should be set when using "Use_limited_type.Many_codes_one_use". Omit if sending your own codes
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Generate_codes_count { get; set; }
        public string Currency_code { get; set; }
        public long? Product_id { get; private set; }
        public int? Package_id { get; private set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Percentage_discount { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Fixed_discount { get; set; }
    }
}
