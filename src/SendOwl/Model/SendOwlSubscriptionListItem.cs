using Newtonsoft.Json;

namespace SendOwl.Model
{
    public class SendOwlSubscriptionListItem : IListItem<SendOwlSubscription>
    {
        [JsonProperty(PropertyName = "subscription")]
        public SendOwlSubscription Value { get; set; }
    }
}
