using Newtonsoft.Json;

namespace Netatmo.Net.Model
{
    public class AlarmConfig
    {
        [JsonProperty(PropertyName = "default_alarm")]
        public DefaultAlarm[] DefaultAlarm { get; set; }

        [JsonProperty(PropertyName = "personnalized")]
        public PersonalizedAlarm[] PersonalizedAlarm { get; set; }
    }
}
