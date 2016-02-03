using Newtonsoft.Json;

namespace Netatmo.Net.Model
{
    public class DashboardData
    {
        [JsonProperty(PropertyName = "AbsolutePressure")]
        public float AbsolutePressure { get; set; }

        [JsonProperty(PropertyName = "Pressure")]
        public float Pressure { get; set; }

        [JsonProperty(PropertyName = "CO2")]
        public float CO2 { get; set; }

        [JsonProperty(PropertyName = "Humidity")]
        public float Humidity { get; set; }

        [JsonProperty(PropertyName = "Noise")]
        public float Noise { get; set; }

        [JsonProperty(PropertyName = "Temperature")]
        public float Temperature { get; set; }

        [JsonProperty(PropertyName = "WindAngle")]
        public int WindAngle { get; set; }

        [JsonProperty(PropertyName = "WindStrength")]
        public int WindStrength { get; set; }

        [JsonProperty(PropertyName = "GustAngle")]
        public int GustAngle { get; set; }

        [JsonProperty(PropertyName = "GustStrength")]
        public int GustStrength { get; set; }

        [JsonProperty(PropertyName = "date_max_temp")]
        public long DateMaxTemp { get; set; }

        [JsonProperty(PropertyName = "date_min_temp")]
        public long DateMinTemp { get; set; }

        [JsonProperty(PropertyName = "date_max_wind_str")]
        public long DateMaxWindStr { get; set; }

        [JsonProperty(PropertyName = "max_wind_angle")]
        public int MaxWindAngle { get; set; }

        [JsonProperty(PropertyName = "max_wind_str")]
        public int MaxWindStr { get; set; }

        [JsonProperty(PropertyName = "WindHistoric")]
        public WindHistoric[] WindHistoric { get; set; }

        [JsonProperty(PropertyName = "max_temp")]
        public float MaxTemp { get; set; }

        [JsonProperty(PropertyName = "min_temp")]
        public float MinTemp { get; set; }

        [JsonProperty(PropertyName = "temp_trend")]
        public string TempTrend { get; set; }

        [JsonProperty(PropertyName = "time_utc")]
        public long TimeUtc { get; set; }
    }
}
