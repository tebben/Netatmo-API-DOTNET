using Newtonsoft.Json;

namespace Netatmo.Net.Model
{
    public class Device
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id{ get; set; }

        [JsonProperty(PropertyName = "alarm_config")]
        public AlarmConfig AlarmConfig { get; set; }

        [JsonProperty(PropertyName = "cipher_id")]
        public string CipherId { get; set; }

        [JsonProperty(PropertyName = "co2_calibrating")]
        public bool Co2Calibrating { get; set; }

        [JsonProperty(PropertyName = "dashboard_data")]
        public DashboardData DashboardData { get; set; }

        [JsonProperty(PropertyName = "data_type")]
        public string[] DataType { get; set; }

        [JsonProperty(PropertyName = "firmware")]
        public int Firmware { get; set; }

        [JsonProperty(PropertyName = "last_status_store")]
        public long LastStatusStore { get; set; }

        [JsonProperty(PropertyName = "last_upgrade")]
        public long LastUpgrade { get; set; }

        [JsonProperty(PropertyName = "meteo_alarms")]
        public MeteoAlarm[] MeteoAlarms { get; set; }

        [JsonProperty(PropertyName = "module_name")]
        public string ModuleName { get; set; }

        [JsonProperty(PropertyName = "modules")]
        public Module[] Modules { get; set; }

        [JsonProperty(PropertyName = "place")]
        public Place Place { get; set; }

        [JsonProperty(PropertyName = "station_name")]
        public string StationName { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "wifi_status")]
        public int WifiStatus { get; set; }
    }
}
