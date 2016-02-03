using System;
using Newtonsoft.Json;

namespace Netatmo.Net.Model
{
    public class OAuthAccessToken
    {
        //To be safe extract some seconds from the original expire seconds
        private int _minusExpiresSeconds = 1;
        private string _accessToken;        
        private DateTime _creationTime;

        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken
        {
            get
            {
                return _accessToken;
            }
            set
            {
                _accessToken = value;
                _creationTime = DateTime.Now;
            }
        }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty(PropertyName = "scope")]
        public string[] Scope { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "expire_in")]
        public long ExpireIn { get; set; }

        [JsonIgnore]
        public bool NeedsRefresh => _creationTime.AddSeconds(ExpiresIn - _minusExpiresSeconds) <= DateTime.Now;
    }
}
