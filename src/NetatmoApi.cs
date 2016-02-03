using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Netatmo.Net.Extensions;
using Netatmo.Net.Model;
using Netatmo.Net.Utils;
using Newtonsoft.Json;

namespace Netatmo.Net
{
    public delegate void LoginSuccessfulHandler(object sender);
    public delegate void LoginFailedlHandler(object sender);

    public class NetatmoApi
    {
        private readonly HttpClient _httpClient;

        public string ClientId { get; }
        public string ClientSecret { get; }
        public OAuthAccessToken OAuthAccessToken { get; private set; }

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
            var content = HttpContentCreator.CreateLoginHttpContent(ClientId, ClientSecret, email, password, scopes);
            var response = await Request<OAuthAccessToken>(Urls.RequestTokenUrl, content, true);

            if (response.Success)
            {
                OAuthAccessToken = response.Result;
                OnLoginSuccessful();
            }
            else
            {
                OnLoginFailed();
            }
        }

        /// <summary>
        /// Retrieve private or partner weather station data
        /// </summary>
        /// <param name="deviceId">leave null or empty to get all devices, specify to get target device</param>
        /// <param name="getfavorites">set to true to get favorited devices</param>
        public async Task<Response<StationsData>> GetStationsData(string deviceId = null, bool getfavorites = false)
        {
            var content = HttpContentCreator.CreateGetStationsDataHttpContent(deviceId, getfavorites);
            var response = await Request<StationsData>(Urls.GetStationsDataUrl, content);
            return response;
        }

        /// <summary>
        /// Retrieve measurements
        /// </summary>
        /// <param name="deviceId">device id to retrieve measurements for</param>        
        /// <param name="scale">Scale for the measurements, not all Measurement Types support all scales for more info see "https://dev.netatmo.com/doc/methods/getmeasure"</param>
        /// <param name="measurementTypes">Measurement types to retrieve</param>
        /// <param name="onlyLastMeasurement">Retrieve only the last measurement</param>
        /// <param name="moduleId">id of the module to retirve if not supplied the measurements from the device will be fetched</param>
        /// <param name="optimize">optimize response data</param>
        /// <param name="begin">begin date of measurements to retrieve</param>
        /// <param name="end">end date of measurements to retrieve</param>
        /// <param name="limit">limit the measurements by a given amount, max = 1024</param>
        /// <param name="realtime">In scales higher than max, since the data is aggregated, the timestamps returned are by default offset by +(scale/2). </param>
        public async Task<Response<MeasurementData>> GetMeasure(string deviceId, Scale scale, MeasurementType[] measurementTypes, string moduleId = null, bool onlyLastMeasurement = false, DateTime? begin = null, DateTime? end = null, bool optimize = true, int limit = 1024, bool realtime = false)
        {
            var content = HttpContentCreator.CreateGetMeasureHttpContent(deviceId, scale, measurementTypes, moduleId, onlyLastMeasurement, begin, end, optimize, limit, realtime);
            var response = await Request<MeasurementData>(Urls.GetMeasureUrl, content);
            if(response.Success)
                response.Result.CreateMeasurementData(optimize, measurementTypes);

            return response;
        }

        private async Task<bool> RefreshToken()
        {
            var content = HttpContentCreator.CreateRefreshTokenHttpContent(OAuthAccessToken.RefreshToken, ClientId, ClientSecret);
            var response = await Request<OAuthAccessToken>(Urls.RequestTokenUrl, content, true);
            if (!response.Success)
                return false;

            OAuthAccessToken = response.Result;
            return true;
        }

        private async Task<Response<T>> Request<T>(string url, Dictionary<string, string> content, bool isTokeRenquest = false)
        {
            var httpContent = new MultipartFormDataContent();

            //Check if authenticated and refresh oauth token if needed
            if (!isTokeRenquest)
            {
                if(OAuthAccessToken == null)
                    return Response<T>.CreateUnsuccessful("Please login first", HttpStatusCode.Unauthorized);

                if (OAuthAccessToken.NeedsRefresh)
                {
                    var refreshed = await RefreshToken();
                    if (!refreshed)
                        return Response<T>.CreateUnsuccessful("Unable to refresh token", HttpStatusCode.ExpectationFailed);
                }

                httpContent.Add(new StringContent(OAuthAccessToken.AccessToken), "access_token");
            }
            
            foreach (var key in content.Keys)
            {
                httpContent.Add(new StringContent(content[key]), key);
            }

            var clientResponse = await _httpClient.PostAsync(url, httpContent);
            var responseString = await clientResponse.Content.ReadAsStringAsync();
            var responseResult = clientResponse.IsSuccessStatusCode ?
                Response<T>.CreateSuccessful(JsonConvert.DeserializeObject<T>(responseString), clientResponse.StatusCode) :
                Response<T>.CreateUnsuccessful(responseString, clientResponse.StatusCode);

            return responseResult;
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