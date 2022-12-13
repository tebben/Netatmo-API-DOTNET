using Newtonsoft.Json;

namespace Netatmo.Net.Model
{
    public class Administrative
    {
        public string country { get; set; }
        public string reg_locale { get; set; }
        public string lang { get; set; }
        public int unit { get; set; }
        public int windunit { get; set; }
        public int pressureunit { get; set; }
        public int feel_like_algo { get; set; }
    }
}
