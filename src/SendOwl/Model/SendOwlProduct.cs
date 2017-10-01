using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace SendOwl.Model
{
    public class SendOwlProduct
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long Id { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ProductType Product_type { get; set; }
        public string Name { get; set; }
        public bool? Pdf_stamping { get; set; }
        public object Sales_limit { get; set; }
        public string Self_hosted_url { get; set; }
        public string License_type { get; set; }
        public object License_fetch_url { get; set; }
        public long? Shopify_variant_id { get; set; }
        public object Custom_field { get; set; }
        public bool Price_is_minimum { get; set; }
        public bool Limit_to_single_qty_in_cart { get; set; }
        public object Download_folder { get; set; }
        public bool Affiliate_sellable { get; set; }
        public decimal? Commission_rate { get; set; }
        public object Weight { get; set; }
        public DateTime Created_at { get; private set; }
        public DateTime Updated_at { get; private set; }
        public string Price { get; set; }
        public string Currency_code { get; private set; }
        public string Product_image_url { get; set; }
        [JsonIgnore]
        public Attachment Attachment { get; private set; }
        [JsonProperty(nameof(Attachment))]
        public Attachment AttachmentInternal { set { Attachment = value; } }
        public string Instant_buy_url { get; private set; }
        public string Add_to_cart_url { get; private set; }
    }
}
