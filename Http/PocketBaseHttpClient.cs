using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimplePocketBase.Auth;

namespace SimplePocketBase.Http
{
    public class PocketBaseHttpClient
    {
        private HttpClient _httpClient;
        private AuthState _authState;
        private string _baseUrl;
        private PocketBaseOptions _options;

        public PocketBaseHttpClient(string baseUrl, HttpClient httpClient, AuthState authState, PocketBaseOptions options)
        {
            _baseUrl = baseUrl.TrimEnd('/');
            _httpClient = httpClient;
            _authState = authState;
            _options = options ?? PocketBaseOptions.CreateDefault();
        }

        internal JsonSerializerSettings JsonSettings
        {
            get { return _options.JsonSettings; }
        }

        public async Task<T> SendAsync<T>(HttpMethod method, string path, object body, CancellationToken cancellationToken = default)
        {
            using (var request = new HttpRequestMessage(method, BuildUrl(path)))
            {
                if (!string.IsNullOrEmpty(_authState.Token))
                {
                    request.Headers.Add("Authorization", _authState.Token);
                }

                if (body != null)
                {
                    var json = JsonConvert.SerializeObject(body, _options.JsonSettings);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                using (var response = await _httpClient.SendAsync(request, cancellationToken))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new PocketBaseException("PocketBase request failed.", response.StatusCode, responseBody);
                    }

                    if (string.IsNullOrWhiteSpace(responseBody))
                    {
                        return default;
                    }

                    return JsonConvert.DeserializeObject<T>(responseBody, _options.JsonSettings);
                }
            }
        }

        private string BuildUrl(string path)
        {
            return _baseUrl + "/api/" + path.TrimStart('/');
        }
    }
}
