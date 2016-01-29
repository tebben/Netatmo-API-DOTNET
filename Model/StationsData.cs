using Newtonsoft.Json;

namespace Netatmo.Net.Model
{
    public class StationsData
    {
        [JsonProperty(PropertyName = "body")]
        public StationsBody Data { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status{ get; set; }

        [JsonProperty(PropertyName = "time_exec")]
        public float TimeExec { get; set; }

        [JsonProperty(PropertyName = "time_server")]
        public long TimeServer { get; set; }
    }
}
