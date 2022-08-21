using Newtonsoft.Json;
using System.Collections.Generic;

namespace Netatmo.Net.Model
{
    public class Body
    {
        public List<Device> devices { get; set; }
        public User user { get; set; }
    }
}
