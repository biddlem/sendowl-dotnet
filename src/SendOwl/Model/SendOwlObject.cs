using Newtonsoft.Json;

namespace SendOwl.Model
{
    public abstract class SendOwlObject<T>
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public abstract T Id { get; set; }
    }
}
