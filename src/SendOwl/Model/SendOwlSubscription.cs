using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SendOwl.Model
{
    public class SendOwlSubscription : SendOwlObject<int>
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public override int Id { get; set; }
        public string Name { get; set; }
        public bool Access_all_products { get; set; }
        public bool Access_subset_products { get; set; }
        public bool? Sell_tangible_product { get; set; }
        public bool? Sell_service { get; set; }
        public bool Perform_redirect { get; set; }
        public string Redirect_url { get; set; }
        public string Custom_field { get; set; }
        public bool Affiliate_sellable { get; set; }
        public string Commission_rate { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime Created_at { get; private set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime Updated_at { get; private set; }
        public string Trial_price { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Frequency? Trial_frequency { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Trial_no_of_occurrences { get; set; }
        public string Recurring_price { get; set; }
        //public Frequency? Frequency { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Frequency_value { get; set; }
        public FrequencyInterval? Frequency_interval { get; set; }
        public object No_of_occurrences { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PaymentType Recurring_type { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Currency_code { get; private set; }
        public Components Components { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Product_image_url { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Instant_buy_url { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<object> Drip_items { get; set; }
    }
}
