using Newtonsoft.Json;
using System.Collections.Generic;

namespace Netatmo.Net.Model
{
    public class AlarmConfig
    {
        public List<DefaultAlarm> default_alarm { get; set; }
        public List<object> personnalized { get; set; }
    }
}
