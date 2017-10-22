using Newtonsoft.Json;

namespace SendOwl.Model
{
    public class SendOwlDiscountListItem : IListItem<SendOwlDiscount>
    {
        [JsonProperty(PropertyName = "discount_code")]
        public SendOwlDiscount Value { get; set; }
    }
}
