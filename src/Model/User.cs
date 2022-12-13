using Newtonsoft.Json;

namespace Netatmo.Net.Model
{
    public class User
    {
        public string mail { get; set; }
        public Administrative administrative { get; set; }
        public bool app_telemetry { get; set; }
    }
}
