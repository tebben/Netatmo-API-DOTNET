﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Netatmo.Net.Extensions;
using Netatmo.Net.Model;
using Netatmo.Net.Utils;
using Newtonsoft.Json;

namespace Netatmo.Net
{
    public delegate void LoginSuccessfulHandler(object sender);
    public delegate void LoginFailedlHandler(object sender);
    public delegate void RefreshTokenSuccessfulHandler(object sender);
    public delegate void RefreshTokenFailedlHandler(object sender);

    public class NetatmoApi
    {
        private readonly HttpClient _httpClient;

        public string ClientId { get; }
        public string ClientSecret { get; }
        public OAuthAccessToken OAuthAccessToken { get; set; }

        public event LoginSuccessfulHandler LoginSuccessful;
        public event LoginFailedlHandler LoginFailed;

        public event RefreshTokenSuccessfulHandler RefreshTokenSuccessful;
        public event RefreshTokenFailedlHandler RefreshTokenFailed;

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
        /// retrieve an OAuthToken by using Authorization
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="scopes"></param>
        public async void AuthorizationCode(string code, string redirect_uri, NetatmoScope[] scopes)
        {
            var content = HttpContentCreator.CreateAuthorizationRequestHttpContent(ClientId, ClientSecret, code, redirect_uri, scopes);
            var response = await Request<OAuthAccessToken>(Urls.RequestTokenUrl, content, true);
            if (response.Success)
            {
                OAuthAccessToken = response.Result;
                OAuthAccessToken.CreationTime = response.Result._creationTime;
                OAuthAccessToken.MinusExpiresSeconds = response.Result._minusExpiresSeconds;
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
            if (response.Success)
                response.Result.CreateMeasurementData(optimize, measurementTypes);

            return response;
        }

        private async Task<bool> RefreshToken()
        {
            var content = HttpContentCreator.CreateRefreshTokenHttpContent(OAuthAccessToken.RefreshToken, ClientId, ClientSecret);
            var response = await Request<OAuthAccessToken>(Urls.RequestTokenUrl, content, true);
            if (!response.Success)
            {
                OnRefreshTokenFailed();
                return false;
            }
            else
            {
                OAuthAccessToken = response.Result;
                OAuthAccessToken.CreationTime = response.Result._creationTime;
                OAuthAccessToken.MinusExpiresSeconds = response.Result._minusExpiresSeconds;
                OnRefreshTokenSuccessful();
                return true;
            }


        }

        private async Task<Response<T>> Request<T>(string url, Dictionary<string, string> content, bool isTokeRenquest = false, string payload = "-1")
        {
            var httpContent = new MultipartFormDataContent();
            var httpRequestMessage = new HttpRequestMessage();
            HttpResponseMessage clientResponse;

            //Check if authenticated and refresh oauth token if needed
            if (!isTokeRenquest)
            {
                if (OAuthAccessToken == null)
                    return Response<T>.CreateUnsuccessful("Please login first", HttpStatusCode.Unauthorized);

                if (OAuthAccessToken.NeedsRefresh)
                {
                    var refreshed = await RefreshToken();
                    if (!refreshed)
                        return Response<T>.CreateUnsuccessful("Unable to refresh token", HttpStatusCode.ExpectationFailed);
                }

                httpContent.Add(new StringContent(OAuthAccessToken.AccessToken), "access_token");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OAuthAccessToken.AccessToken);
            }

            foreach (var key in content.Keys)
            {
                httpContent.Add(new StringContent(content[key]), key);
            }


            if (payload != "-1")
            {
                httpRequestMessage.Method = HttpMethod.Post;
                httpRequestMessage.RequestUri = new Uri(url);
                //httpRequestMessage.Headers.TryAddWithoutValidation("Authorization", "Bearer " + OAuthAccessToken.AccessToken);
                //httpRequestMessage.Headers.TryAddWithoutValidation("accept", "application/json");
                httpRequestMessage.Content = new StringContent(payload, Encoding.UTF8, "application/json");//CONTENT-TYPE header

                clientResponse = _httpClient.SendAsync(httpRequestMessage).Result;
            }
            else
            {
                clientResponse = await _httpClient.PostAsync(url, httpContent);
            }


            var responseString = await clientResponse.Content.ReadAsStringAsync();
            var responseResult = clientResponse.IsSuccessStatusCode ?
                Response<T>.CreateSuccessful(JsonConvert.DeserializeObject<T>(responseString), clientResponse.StatusCode) :
                Response<T>.CreateUnsuccessful(responseString, clientResponse.StatusCode);

            return responseResult;
        }


        /* iDiamant */

        /// <summary>
        /// Retrieve private or partner weather station data
        /// </summary>
        /// <param name="homeid">leave null or empty to get all, specify to Filter by Home ID</param>
        /// <param name="gatewaytypes">leave null or empty to get all, specify to Filter by Gateway Type {BNS, NLG, OTH, NBG}</param>
        public async Task<Response<iDiamantHomes.Root>> GetiDiamantHomesData(string homeid = null, iDiamantGatewayTypes[] gatewaytypes = null)
        {
            var content = HttpContentCreator.CreateGetiDiamantHomesDataHttpContent(homeid);
            var response = await Request<iDiamantHomes.Root>(Urls.GetHomesDataUrl, content);
            return response;
        }

        /// <summary>
        /// Retrieve private or partner weather station data
        /// </summary>
        /// <param name="homeid">mandatory, specify to Filter by Home ID</param>
        /// <param name="gatewaytypes">leave null or empty to get all, specify to Filter by Gateway Type {BNS, NLG, OTH, NBG}</param>
        public async Task<Response<iDiamantHomeStatus.Root>> GetiDiamantHomeStatusData(string homeid, iDiamantGatewayTypes[] gatewaytypes = null)
        {
            var content = HttpContentCreator.CreateGetiDiamantHomeStatusDataHttpContent(homeid);
            string uri = Urls.GetHomeStatusDataUrl;
            uri = uri.Replace("%id%", homeid);
            var response = await Request<iDiamantHomeStatus.Root>(uri, content);
            return response;
        }


        public async Task<Response<iDiamantSetShutter.Root>> SetShutter(string home_id, string module_id, int module_targetposition, string module_bridge)
        {
            var content = HttpContentCreator.CreateSetShutterDataHttpContent(home_id, module_id, module_targetposition, module_bridge);
            //string payload = "{\"home\":{\"id\":\"5e9a0832a91a648422152f28\",\"modules\":[{\"id\":\"0000259417\",\"target_position\":100,\"bridge\":\"70:ee:50:82:9d:ae\"}]}}";
            string payload = "{\"home\":{\"id\":\"%homeid%\",\"modules\":[{\"id\":\"%shutterid%\",\"target_position\":%target_position%,\"bridge\":\"%bridgeid%\"}]}}";
            payload = payload.Replace("%homeid%", home_id);
            payload = payload.Replace("%target_position%", module_targetposition.ToString());
            payload = payload.Replace("%shutterid%", module_id);
            payload = payload.Replace("%bridgeid%", module_bridge);
            var response = await Request<iDiamantSetShutter.Root>(Urls.SetHomeSetShutterUrl, content, false, payload);
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

        protected virtual void OnRefreshTokenSuccessful()
        {
            RefreshTokenSuccessful?.Invoke(this);
        }

        protected virtual void OnRefreshTokenFailed()
        {
            RefreshTokenFailed?.Invoke(this);
        }

    }
}