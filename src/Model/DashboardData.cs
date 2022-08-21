using Newtonsoft.Json;

namespace Netatmo.Net.Model
{
    public class DashboardData
    {
        public int time_utc { get; set; }
        public double Temperature { get; set; }
        public double CO2 { get; set; }
        public double Humidity { get; set; }
        public double Noise { get; set; }
        public double Pressure { get; set; }
        public double AbsolutePressure { get; set; }
        public double min_temp { get; set; }
        public double max_temp { get; set; }
        public int date_max_temp { get; set; }
        public int date_min_temp { get; set; }
        public string temp_trend { get; set; }
        public string pressure_trend { get; set; }
        public int? Rain { get; set; }
        public double? sum_rain_1 { get; set; }
        public double? sum_rain_24 { get; set; }
    }
}
