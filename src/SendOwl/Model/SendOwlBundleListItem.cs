using Newtonsoft.Json;

namespace SendOwl.Model
{
    public class SendOwlBundleListItem : IListItem<SendOwlBundle>
    {
        [JsonProperty("package")]
        public SendOwlBundle Value { get; set; }
    }
}
