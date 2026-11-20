using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SimplePocketBase.Http;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace SimplePocketBase.Auth
{
    public class AuthService
    {
        private PocketBaseHttpClient _http;
        private AuthState _authState;

        internal AuthService(PocketBaseHttpClient http, AuthState authState)
        {
            _http = http;
            _authState = authState;
        }

        public async Task<AuthResponse<T>> LoginAsync<T>(string collection, string identity, string password, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(collection))
            {
                throw new ArgumentException("Collection is required.", "collection");
            }
                
            var payload = new
            {
                identity = identity,
                password = password
            };

            var result = await _http.SendAsync<AuthResponse<T>>(
                HttpMethod.Post,
                "collections/" + Uri.EscapeDataString(collection) + "/auth-with-password",
                payload,
                cancellationToken
            );

            _authState.Token = result.Token;
            return result;
        }

        public void Logout()
        {
            _authState.Token = null;
        }

        public Task<T> RegisterAsync<T>(string collection, object data, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(collection))
            {
                throw new ArgumentException("Collection is required.", "collection");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            var payload = JObject.FromObject(data);

            var password = payload["password"]?.Value<string>();
            var passwordConfirm = payload["passwordConfirm"]?.Value<string>();

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password is required.", "data");
            }
            if (password != passwordConfirm)
            {
                throw new ArgumentException("Passwords do not match.", "data");
            }

            return _http.SendAsync<T>(
                HttpMethod.Post,
                "collections/" + Uri.EscapeDataString(collection) + "/records",
                payload,
                cancellationToken
            );
        }
    }
}
