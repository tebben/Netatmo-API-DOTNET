using Newtonsoft.Json;

namespace Netatmo.Net.Model
{
    public class Module
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "battery_percent")]
        public int BatteryPercent { get; set; }

        [JsonProperty(PropertyName = "battery_vp")]
        public int BatteryVp { get; set; }

        [JsonProperty(PropertyName = "dashboard_data")]
        public DashboardData DashboardData { get; set; }

        [JsonProperty(PropertyName = "data_type")]
        public string[] DataType { get; set; }

        [JsonProperty(PropertyName = "firmware")]
        public int Firmware { get; set; }

        [JsonProperty(PropertyName = "last_message")]
        public long LastMessage { get; set; }

        [JsonProperty(PropertyName = "last_seen")]
        public long LastSeen { get; set; }

        [JsonProperty(PropertyName = "module_name")]
        public string ModuleName { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
