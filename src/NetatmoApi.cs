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

        public string UrlBase = "https://api.netatmo.com";
        public string UrlRequestTokenPath = "/oauth2/token";
        public string UrlGetStationsData = "/api/getstationsdata";

        public OAuthAccessToken OAuthAccessToken { get; private set; }

        public string ClientId { get; }
        public string ClientSecret { get; }

        public event LoginSuccessfulHandler LoginSuccessful;
        public event LoginFailedlHandler LoginFailed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        public NetatmoApi(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            _httpClient = new HttpClient();
        }
        
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
        /// 
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="getfavorites"></param>
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