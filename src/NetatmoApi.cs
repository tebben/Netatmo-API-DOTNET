using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Netatmo.Net.Extensions;
using Netatmo.Net.Model;
using Newtonsoft.Json;

namespace Netatmo.Net
{
    public delegate void LoginSuccessfulHandler(object sender);
    public delegate void LoginFailedlHandler(object sender);

    public class NetatmoApi
    {
        private readonly HttpClient _httpClient;
        private string RequestTokenUrl => $"{UrlBase}{UrlRequestTokenPath}";
        private string GetStationsDataUrl => $"{UrlBase}{UrlGetStationsData}";
        private string GetMeasureUrl => $"{UrlBase}{UrlGetMeasuree}";

        public string UrlBase = "https://api.netatmo.com";
        public string UrlRequestTokenPath = "/oauth2/token";
        public string UrlGetStationsData = "/api/getstationsdata";
        public string UrlGetMeasuree = "/api/getmeasure";

        public OAuthAccessToken OAuthAccessToken { get; private set; }

        public string ClientId { get; }
        public string ClientSecret { get; }

        public event LoginSuccessfulHandler LoginSuccessful;
        public event LoginFailedlHandler LoginFailed;

        /// <summary>
        /// Create the Netatmo API
        /// </summary>
        /// <param name="clientId">application OAuth Client id, found under "https://dev.netatmo.com/"</param>
        /// <param name="clientSecret">application OAuth Client secret, found under "https://dev.netatmo.com/"</param>
        public NetatmoApi(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            _httpClient = new HttpClient();
        }
        
        /// <summary>
        /// Login to netatmo and retrieve an OAuthToken
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="scopes"></param>
        public async void Login(string email, string password, NetatmoScope[] scopes)
        {
            var content = new MultipartFormDataContent
            {
                {new StringContent("password"), "grant_type"},
                {new StringContent(ClientId), "client_id"},
                {new StringContent(ClientSecret), "client_secret"},
                {new StringContent(email), "username"},
                {new StringContent(password), "password"},
                {new StringContent(scopes.ToScopeString()), "scope"}
            };

            var response = await _httpClient.PostAsync(RequestTokenUrl, content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                OAuthAccessToken = JsonConvert.DeserializeObject<OAuthAccessToken>(responseString);
                OnLoginSuccessful();
            }
            else
            {
                OnLoginFailed();
            }
        }

        /// <summary>
        /// Retrieve stations data
        /// </summary>
        /// <param name="deviceId">leave null or empty to get all devices, specify to get target device</param>
        /// <param name="getfavorites">set to true to get favorited devices</param>
        public async Task<Response<StationsData>> GetStationsData(string deviceId = null, bool getfavorites = false)
        {
            var canRequest = await CanRequestNetatmo();
            if(!canRequest.IsSuccessStatusCode)
                return Response<StationsData>.CreateUnsuccessful("Unable to request Netatmo", canRequest.StatusCode);

            var content = new MultipartFormDataContent
            {
                {new StringContent(OAuthAccessToken.AccessToken), "access_token"},
            };

            if(!string.IsNullOrEmpty(deviceId))
                content.Add(new StringContent(deviceId), "device_id");
            if (getfavorites)
                content.Add(new StringContent("true"), "get_favorites");

            var response = await _httpClient.PostAsync(GetStationsDataUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();

            var sdResponse = response.IsSuccessStatusCode ? 
                Response<StationsData>.CreateSuccessful(JsonConvert.DeserializeObject<StationsData>(responseString), response.StatusCode) : 
                Response<StationsData>.CreateUnsuccessful(responseString, response.StatusCode);

            return sdResponse;
        }

        /// <summary>
        /// Retrieve measurements
        /// </summary>
        /// <param name="deviceId">leave null or empty to get all devices, specify to get target device</param>        
        /// <param name="scale"></param>
        /// <param name="measurementTypes"></param>
        /// <param name="onlyLastMeasurement"></param>
        /// <param name="moduleId"></param>
        /// <param name="optimize"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="limit"></param>
        /// <param name="realtime"></param>
        public async Task<Response<MeasurementData>> GetMeasure(string deviceId, Scale scale, MeasurementType[] measurementTypes, bool onlyLastMeasurement = false, string moduleId = null, bool optimize = true, DateTime? begin = null, DateTime? end = null, int limit = 1024, bool realtime = false)
        {
            var canRequest = await CanRequestNetatmo();
            if (!canRequest.IsSuccessStatusCode)
                return Response<MeasurementData>.CreateUnsuccessful("Unable to request Netatmo", canRequest.StatusCode);

            var content = new MultipartFormDataContent
            {
                { new StringContent(OAuthAccessToken.AccessToken), "access_token"},
                { new StringContent(deviceId), "device_id" },
                { new StringContent(scale.GetScaleName()), "scale" },
                { new StringContent(measurementTypes.ToMeasurementTypesString()), "type" },
                { new StringContent(optimize.ToString()), "optimize" },
            };

            if (!string.IsNullOrEmpty(moduleId))
                content.Add(new StringContent(moduleId), "module_id");
            if (onlyLastMeasurement)
            {
                content.Add(new StringContent("last"), "date_end");
            }
            else
            {
                if (begin.HasValue)
                    content.Add(new StringContent(begin.Value.ToUtcTimestamp().ToString()), "date_begin");
                if (end.HasValue)
                    content.Add(new StringContent(end.Value.ToUtcTimestamp().ToString()), "date_end");
            }
           
            if (limit != 1024)
                content.Add(new StringContent(limit > 1024 || limit < 1 ? "1024" : limit.ToString()), "limit");
            if (realtime)
                content.Add(new StringContent("true"), "realtime");

            var response = await _httpClient.PostAsync(GetMeasureUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();

            var sdResponse = response.IsSuccessStatusCode ?
                Response<MeasurementData>.CreateSuccessful(JsonConvert.DeserializeObject<MeasurementData>(responseString), response.StatusCode) :
                Response<MeasurementData>.CreateUnsuccessful(responseString, response.StatusCode);

            if(response.IsSuccessStatusCode)
                sdResponse.Result.CreateMeasurementData(optimize, measurementTypes);

            return sdResponse;
        }

        private async Task<HttpResponseMessage> CanRequestNetatmo()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            if (OAuthAccessToken == null)
                throw new Exception("Please login first");

            if (OAuthAccessToken.NeedsRefresh)
                response = await RefreshToken();

            return response;
        }

        private async Task<HttpResponseMessage> RefreshToken()
        {
            var content = new MultipartFormDataContent
            {
                {new StringContent("refresh_token"), "grant_type"},
                {new StringContent(OAuthAccessToken.RefreshToken), "refresh_token"},
                {new StringContent(ClientId), "client_id"},
                {new StringContent(ClientSecret), "client_secret"},
            };

            var response = await _httpClient.PostAsync(RequestTokenUrl, content);
            if (!response.IsSuccessStatusCode)
                return response;

            var responseString = await response.Content.ReadAsStringAsync();
            OAuthAccessToken = JsonConvert.DeserializeObject<OAuthAccessToken>(responseString);

            return response;
        }

        protected virtual void OnLoginSuccessful()
        {
            LoginSuccessful?.Invoke(this);
        }

        protected virtual void OnLoginFailed()
        {
            LoginFailed?.Invoke(this);
        }
    }
}