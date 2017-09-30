using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SendOwl
{
    public class LowercaseJsonSerializer : JsonSerializer
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static string SerializeObject(object o)
        {
            return JsonConvert.SerializeObject(o, Formatting.None, Settings);
        }

        public static T DeserializeObject<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content, Settings);
        }
    }
}
