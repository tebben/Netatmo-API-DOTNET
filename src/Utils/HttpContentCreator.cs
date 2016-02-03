using System;
using System.Collections.Generic;
using Netatmo.Net.Extensions;
using Netatmo.Net.Model;

namespace Netatmo.Net.Utils
{
    public class HttpContentCreator
    {
        public static Dictionary<string, string> CreateLoginHttpContent(string clientId, string clientSecret, string email, string password, NetatmoScope[] scopes)
        {
            var content = new Dictionary<string, string>
            {
                {"grant_type", "password"},
                {"client_id", clientId},
                {"client_secret", clientSecret},
                {"username", email},
                {"password", password},
                {"scope", scopes.ToScopeString()}
            };

            return content;
        }

        public static Dictionary<string, string> CreateRefreshTokenHttpContent(string refreshToken, string clientId, string clientSecret)
        {
            var content = new Dictionary<string, string>
                {
                    {"grant_type", "refresh_token"},
                    {"refresh_token", refreshToken },
                    {"client_id",  clientId},
                    {"client_secret", clientSecret}
                };

            return content;
        }

        public static Dictionary<string, string> CreateGetStationsDataHttpContent(string deviceId = null, bool getfavorites = false)
        {
            var content = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(deviceId))
                content.Add("device_id", deviceId);
            if (getfavorites)
                content.Add("get_favorites", "true");

            return content;
        }

        public static Dictionary<string, string> CreateGetMeasureHttpContent(string deviceId, Scale scale, MeasurementType[] measurementTypes, string moduleId = null, bool onlyLastMeasurement = false, DateTime? begin = null, DateTime? end = null, bool optimize = true, int limit = 1024, bool realtime = false)
        {
            var content = new Dictionary<string, string>
            {
                {"device_id", deviceId },
                {"scale",  scale.GetScaleName()},
                {"type", measurementTypes.ToMeasurementTypesString() },
                {"optimize", optimize.ToString() },
            };

            if (!string.IsNullOrEmpty(moduleId))
                content.Add("module_id", moduleId);
            if (onlyLastMeasurement)
            {
                content.Add("date_end", "last");
            }
            else
            {
                if (begin.HasValue)
                    content.Add("date_begin", begin.Value.ToUtcTimestamp().ToString());
                if (end.HasValue)
                    content.Add("date_end", end.Value.ToUtcTimestamp().ToString());
            }

            if (limit != 1024)
                content.Add("limit", limit > 1024 || limit < 1 ? "1024" : limit.ToString());
            if (realtime)
                content.Add("realtime", "true");

            return content;
        }
    }
}
