using Newtonsoft.Json;

namespace Netatmo.Net.Model
{
    public class DefaultAlarm
    {
        [JsonProperty(PropertyName = "db_alarm_number")]
        public int DbAlarmNumber { get; set; }

        [JsonProperty(PropertyName = "desactivated")]
        public bool Desactivated { get; set; }
    }
}
