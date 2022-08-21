using Newtonsoft.Json;
using System.Collections.Generic;

namespace Netatmo.Net.Model
{
    public class Place
    {
        public int altitude { get; set; }
        public string city { get; set; }
        public string street { get; set; }
        public string country { get; set; }
        public string timezone { get; set; }
        public List<double> location { get; set; }
    }
}
