using Newtonsoft.Json;

namespace SendOwl.Model
{
    public class SendOwlProductListItem : IListItem<SendOwlProduct>
    {
        [JsonProperty(PropertyName = "product")]
        public SendOwlProduct Value { get; set; }
    }
}
