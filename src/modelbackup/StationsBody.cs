using Newtonsoft.Json;

namespace Netatmo.Net.Model
{
    public class StationsBody
    {
        [JsonProperty(PropertyName = "devices")]
        public Device[] Devices { get; set; }

        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }
    }
}
