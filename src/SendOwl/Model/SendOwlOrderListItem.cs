using Newtonsoft.Json;

namespace SendOwl.Model
{
    public class SendOwlOrderListItem: IListItem<SendOwlOrder>
    {
        [JsonProperty(PropertyName = "order")]
        public SendOwlOrder Value { get; set; }
    }
}
