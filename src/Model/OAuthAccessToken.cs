﻿using System;
using Newtonsoft.Json;

namespace Netatmo.Net.Model
{
    public class OAuthAccessToken
    {
        //To be safe extract some seconds from the original expire seconds
        public int _minusExpiresSeconds = 1;
        private string _accessToken;        
        public DateTime _creationTime;

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

        [JsonProperty(PropertyName = "creation_time")]
        public DateTime CreationTime { get; set; }

        [JsonProperty(PropertyName = "minusexpires_seconds")]
        public int MinusExpiresSeconds { get; set; }

        [JsonIgnore]
        public bool NeedsRefresh => CreationTime.AddSeconds(ExpiresIn - MinusExpiresSeconds) <= DateTime.Now;
        //public bool NeedsRefresh => _creationTime.AddSeconds(ExpiresIn - _minusExpiresSeconds) <= DateTime.Now;
    }
}
