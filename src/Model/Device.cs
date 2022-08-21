using Newtonsoft.Json;
using System.Collections.Generic;

namespace Netatmo.Net.Model
{
    public class Device
    {
        public string _id { get; set; }
        public string station_name { get; set; }
        public int date_setup { get; set; }
        public int last_setup { get; set; }
        public string type { get; set; }
        public int last_status_store { get; set; }
        public string module_name { get; set; }
        public int firmware { get; set; }
        public int last_upgrade { get; set; }
        public int wifi_status { get; set; }
        public List<string> user_owner { get; set; }
        public bool reachable { get; set; }
        public AlarmConfig alarm_config { get; set; }
        public bool co2_calibrating { get; set; }
        public int hardware_version { get; set; }
        public string customer_id { get; set; }
        public List<string> data_type { get; set; }
        public Place place { get; set; }
        public bool public_ext_data { get; set; }
        public bool air_quality_available { get; set; }
        public string access_code { get; set; }
        public string home_id { get; set; }
        public string home_name { get; set; }
        public DashboardData dashboard_data { get; set; }
        public List<Module> modules { get; set; }
    }
}
