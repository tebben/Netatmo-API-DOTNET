using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netatmo.Net.Model
{
    public class iDiamantHomeStatus
    {

        public class Module
        {
            public string type { get; set; }
            public int? firmware_revision { get; set; }
            public int wifi_strength { get; set; }
            public int last_status_store { get; set; }
            public bool co2_calibrating { get; set; }
            public string id { get; set; }
            public double Temperature { get; set; }
            public int CO2 { get; set; }
            public int Humidity { get; set; }
            public int Noise { get; set; }
            public double Pressure { get; set; }
            public double AbsolutePressure { get; set; }
            public int time_utc { get; set; }
            public double min_temp { get; set; }
            public double max_temp { get; set; }
            public int date_max_temp { get; set; }
            public int date_min_temp { get; set; }
            public string temp_trend { get; set; }
            public string pressure_trend { get; set; }
            public string homekit_status { get; set; }
            public string wifi_state { get; set; }
            public int? battery_level { get; set; }
            public int? last_message { get; set; }
            public int? last_seen { get; set; }
            public int? rf_strength { get; set; }
            public bool? reachable { get; set; }
            public string bridge { get; set; }
            public string battery_state { get; set; }
            public string rf_state { get; set; }
            public int? Rain { get; set; }
            public int? sum_rain_1 { get; set; }
            public double? sum_rain_24 { get; set; }
            public bool? busy { get; set; }
            public bool? configure { get; set; }
            public bool? configured { get; set; }
            public int? timestamp { get; set; }
            public string bubendorff_motor_type { get; set; }
            public int? current_position { get; set; }

            [JsonProperty("current_position:step")]
            public int? CurrentPositionStep { get; set; }
            public int? last_user_interaction { get; set; }
            public bool? shutter_preferred_pos_enabled { get; set; }
            public bool? shutter_thermal_control_enabled { get; set; }
            public int? target_position { get; set; }

            [JsonProperty("target_position:step")]
            public int? TargetPositionStep { get; set; }
        }

        public class Home
        {
            public string id { get; set; }
            public List<Module> modules { get; set; }
        }

        public class Body
        {
            public Home home { get; set; }
        }

        public class Root
        {
            public string status { get; set; }
            public int time_server { get; set; }
            public Body body { get; set; }
        }

    }
}
