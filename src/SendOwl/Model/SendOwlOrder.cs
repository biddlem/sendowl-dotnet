using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SendOwl.Model
{
    public class SendOwlOrder : SendOwlObject<int>
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public override int Id { get; set; }
        public string State { get; set; }
        public string Gateway { get; set; }
        public string Buyer_email { get; set; }
        public string Paypal_email { get; set; }
        public string Buyer_name { get; set; }
        public string Business_name { get; set; }
        public string Business_vat_number { get; set; }
        public string Buyer_address1 { get; set; }
        public string Buyer_address2 { get; set; }
        public string Buyer_city { get; set; }
        public string Buyer_region { get; set; }
        public string Buyer_postcode { get; set; }
        public string Buyer_country { get; set; }
        public string Buyer_ip_address { get; set; }
        public string Settled_currency { get; set; }
        public string Settled_gateway_fee { get; set; }
        public string Settled_tax { get; set; }
        public string Settled_gross { get; set; }
        public string Settled_handling { get; set; }
        public string Settled_shipping { get; set; }
        public string Settled_affiliate_fee { get; set; }
        public string Affiliate_system { get; set; }
        public bool Can_market_to_buyer { get; set; }
        public DateTime? Valid_until { get; set; }
        public int? Download_attempts { get; set; }
        public bool Access_allowed { get; set; }
        public DateTime? Dispatched_at { get; set; }
        public object Tag { get; set; }
        public string Giftee_name { get; set; }
        public string Giftee_email { get; set; }
        public string Gift_deliver_at { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public List<object> gateway_transaction_ids { get; set; }
        public string Download_url { get; set; }
        public string Subscription_management_url { get; set; }
        public string Discount { get; set; }
        public string Accounted_tax { get; set; }
        public string Price_at_checkout { get; set; }
        public string Eu_resolved_country { get; set; }
        public List<object> Order_custom_checkout_fields { get; set; }
        public List<object> Download_items { get; set; }
        public Cart Cart { get; set; }

    }
}
