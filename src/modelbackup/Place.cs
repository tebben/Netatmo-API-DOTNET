using Newtonsoft.Json;

namespace Netatmo.Net.Model
{
    public class Place
    {
        [JsonProperty(PropertyName = "altitude")]
        public float Altitude { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "geoip_city")]
        public string GeoipCity { get; set; }

        [JsonProperty(PropertyName = "improveLocProposed")]
        public bool ImproveLocProposed { get; set; }

        [JsonProperty(PropertyName = "location")]
        public double[] Location { get; set; }

        [JsonProperty(PropertyName = "timezone")]
        public string TimeZone{ get; set; }
    }
}
