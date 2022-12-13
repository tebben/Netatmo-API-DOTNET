using Newtonsoft.Json;

namespace Netatmo.Net.Model
{
    public class StationsData
    {
        public Body body { get; set; }
        public string status { get; set; }
        public double time_exec { get; set; }
        public int time_server { get; set; }
    }
}
