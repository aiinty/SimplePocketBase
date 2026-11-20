using System;
using System.Net.Http;

namespace SimplePocketBase
{
    public class PocketBaseClient
    {
        private HttpClient _httpClient;
        public HttpClient HttpClient
        {
            get 
            { 
                return _httpClient; 
            }
        }

        public string BaseUrl { get; private set; }

        public PocketBaseClient(string baseUrl) : this(baseUrl, new HttpClient()) { }

        public PocketBaseClient(string baseUrl, HttpClient httpClient)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentException("PocketBase URL is required.", "baseUrl");
            }
                
            BaseUrl = baseUrl.TrimEnd('/');
            _httpClient = httpClient;
        }
    }
}
