using Newtonsoft.Json;

namespace Netatmo.Net.Model
{
    public class Administrative
    {            
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "feel_like_algo")]
        public int FeelLikeAlgo { get; set; }

        [JsonProperty(PropertyName = "lang")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "pressureunit")]
        public int PressureUnit { get; set; }

        [JsonProperty(PropertyName = "reg_locale")]
        public string RegLocale { get; set; }

        [JsonProperty(PropertyName = "unit")]
        public int Unit { get; set; }

        [JsonProperty(PropertyName = "windunit")]
        public int WindUnit { get; set; }
    }
}
