using Newtonsoft.Json;

namespace Netatmo.Net.Model
{
    public class WindHistoric
    {
        [JsonProperty(PropertyName = "WindAngle")]
        public int WindAngle { get; set; }

        [JsonProperty(PropertyName = "WindStrength")]
        public int WindStrength { get; set; }

        [JsonProperty(PropertyName = "time_utc")]
        public long TimeUtc { get; set; }
    }
}
