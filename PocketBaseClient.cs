using System;
using System.Net.Http;
using SimplePocketBase.Auth;
using SimplePocketBase.Http;
using SimplePocketBase.Records;

namespace SimplePocketBase
{
    public class PocketBaseClient : IDisposable
    {
        private HttpClient _httpClient;
        public string BaseUrl { get; private set; }
        public AuthService Auth { get; private set; }
        public RecordService Records { get; private set; }

        public PocketBaseClient(string baseUrl) : this(baseUrl, null) { }

        public PocketBaseClient(string baseUrl, PocketBaseOptions options)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentException("PocketBase URL is required.", "baseUrl");
            }

            BaseUrl = baseUrl.TrimEnd('/');
            _httpClient = new HttpClient();

            var authState = new AuthState();
            var http = new PocketBaseHttpClient(
                BaseUrl,
                _httpClient,
                authState,
                options ?? PocketBaseOptions.CreateDefault()
            );

            Auth = new AuthService(http, authState);
            Records = new RecordService(http);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
