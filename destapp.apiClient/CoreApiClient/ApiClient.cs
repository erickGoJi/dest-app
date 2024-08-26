using destapp.apiClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace destapp.apiClient.CoreApiClient
{
    public partial class ApiClient
    {
        private readonly HttpClient _httpClient;
        private Uri BaseEndpoint { get; set; }
        private Dictionary<String, String> headers = new Dictionary<string, string>();
        public ApiClient()
        {
            BaseEndpoint = new Uri(Constants.API_BASE_URL);
            _httpClient = new HttpClient();
        }

        public Uri CreateRequestUri(object constanst)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthenticationResponse> Authenticate()
        {
            clearHeaders();
            Uri uri = CreateRequestUri("_/auth/authenticate");
            Dictionary<String, String> headers = new Dictionary<string, string>();
            headers.Add("Cookie", "PHPSESSID=qrhag5112j5dvq3dukah9b3m9d");
            setHeaders(headers);
            AuthenticationRequest authenticationRequest = new AuthenticationRequest();
            authenticationRequest.email = "cg@massive.ag";
            authenticationRequest.password = "1q2w3e4r";
            return await PostAsync<AuthenticationResponse,AuthenticationRequest>(uri, authenticationRequest);
        }

        public void SetBaseUri(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("baseEndpoint");
            }
            BaseEndpoint = uri;
        }

        public async Task<T> GetAsync<T>(Uri requestUrl)
        {
            addHeaders();
            var response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(data);
        }

        /// <summary>
        /// Common method for making POST calls
        /// </summary>
        public async Task<T> PostAsync<T>(Uri requestUrl, T content)
        {
            addHeaders();
            var response = await _httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T>(content));
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(data);
        }

        public async Task<T1> PostAsync<T1, T2>(Uri requestUrl, T2 content)
        {
            addHeaders();
            var response = await _httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T2>(content));
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T1>(data);
        }

        public async Task<T> PatchAsync<T>(Uri requestUrl, T content)
        {
            addHeaders();
            var response = await _httpClient.PatchAsync(requestUrl.ToString(), CreateHttpContent(content));
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(data);
        }

        public async Task<T1> PatchAsync<T1, T2>(Uri requestUrl, T2 content)
        {
            addHeaders();
            var response = await _httpClient.PatchAsync(requestUrl.ToString(), CreateHttpContent<T2>(content));
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T1>(data);
        }

        public Uri CreateRequestUri(string relativePath, string queryString = "", string relativePathBase = null)
        {
            if (relativePathBase != null)
                BaseEndpoint = new Uri(relativePathBase);
            var endpoint = new Uri(BaseEndpoint, relativePath);
            var uriBuilder = new UriBuilder(endpoint);
            uriBuilder.Query = queryString;
            return uriBuilder.Uri;
        }

        public HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }

        public void setHeaders(Dictionary<String,String> headers)
        {
            this.headers = headers;
        }

        public void setAuthorizationToken(String token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void clearHeaders()
        {
            _httpClient.DefaultRequestHeaders.Clear();
        }

        public void addHeaders()
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
            foreach (KeyValuePair<String, String> header in headers)
            {
                if(header.Value.Contains("key="))
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                else
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
    }
}
