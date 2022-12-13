using Newtonsoft.Json;
using System.Collections.Generic;

namespace Netatmo.Net.Model
{
    public class Module
    {
        public string _id { get; set; }
        public string type { get; set; }
        public string module_name { get; set; }
        public int last_setup { get; set; }
        public List<string> data_type { get; set; }
        public double pluvio_scale_auget_to_mm { get; set; }
        public int battery_percent { get; set; }
        public string battery_level { get; set; }
        public bool reachable { get; set; }
        public int firmware { get; set; }
        public int last_message { get; set; }
        public int last_seen { get; set; }
        public int rf_status { get; set; }
        public int battery_vp { get; set; }
        public DashboardData dashboard_data { get; set; }
    }
}
