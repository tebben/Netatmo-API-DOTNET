using Newtonsoft.Json;

namespace Netatmo.Net.Model
{
    public class User
    {
        [JsonProperty(PropertyName = "administrative")]
        public Administrative Administrative { get; set; }

        [JsonProperty(PropertyName = "mail")]
        public string Mail { get; set; }
    }
}
