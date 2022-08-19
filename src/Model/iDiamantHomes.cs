using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netatmo.Net.Model
{
    public class iDiamantHomes
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Room
        {
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public List<string> module_ids { get; set; }
        }

        public class Module
        {
            public string id { get; set; }
            public string type { get; set; }
            public string name { get; set; }
            public int setup_date { get; set; }
            public List<string> modules_bridged { get; set; }
            public string bridge { get; set; }
            public string room_id { get; set; }
            public int target_position { get; set; }
        }

        public class Zone
        {
            public string name { get; set; }
            public int id { get; set; }
            public List<object> rooms { get; set; }
            public List<Module> modules { get; set; }
        }

        public class TimetableSunrise
        {
            public int zone_id { get; set; }
            public int day { get; set; }
        }

        public class Schedule
        {
            public List<object> timetable { get; set; }
            public List<Zone> zones { get; set; }
            public string name { get; set; }
            public bool @default { get; set; }
            public List<TimetableSunrise> timetable_sunrise { get; set; }
            public List<object> timetable_sunset { get; set; }
            public string id { get; set; }
            public bool selected { get; set; }
            public string type { get; set; }
        }

        public class Home
        {
            public string id { get; set; }
            public string name { get; set; }
            public int altitude { get; set; }
            public List<double> coordinates { get; set; }
            public string country { get; set; }
            public string timezone { get; set; }
            public List<Room> rooms { get; set; }
            public List<Module> modules { get; set; }
            public string temperature_control_mode { get; set; }
            public string therm_mode { get; set; }
            public int therm_setpoint_default_duration { get; set; }
            public string cooling_mode { get; set; }
            public List<Schedule> schedules { get; set; }
        }

        public class User
        {
            public string email { get; set; }
            public string language { get; set; }
            public string locale { get; set; }
            public int feel_like_algorithm { get; set; }
            public int unit_pressure { get; set; }
            public int unit_system { get; set; }
            public int unit_wind { get; set; }
            public string id { get; set; }
        }

        public class Body
        {
            public List<Home> homes { get; set; }
            public User user { get; set; }
        }

        public class Root
        {
            public Body body { get; set; }
            public string status { get; set; }
            public double time_exec { get; set; }
            public int time_server { get; set; }
        }


    }
}
