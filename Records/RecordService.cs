using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SimplePocketBase.Http;

namespace SimplePocketBase.Records
{
    public class RecordService
    {
        private PocketBaseHttpClient _http;

        internal RecordService(PocketBaseHttpClient http)
        {
            _http = http;
        }

        public Task<PagedResult<T>> GetListAsync<T>(
            string collection,
            int page = 1,
            int perPage = 30,
            string filter = null,
            string sort = null,
            CancellationToken cancellationToken = default
        )
        {
            var query = new List<string>
            {
                "page=" + page,
                "perPage=" + perPage
            };

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query.Add("filter=" + Uri.EscapeDataString(filter));
            }
            if (!string.IsNullOrWhiteSpace(sort))
            {
                query.Add("sort=" + Uri.EscapeDataString(sort));
            }

            return _http.SendAsync<PagedResult<T>>(
                HttpMethod.Get,
                "collections/" + Encode(collection) + "/records?" + string.Join("&", query),
                null,
                cancellationToken
            );
        }

        public Task<T> GetOneAsync<T>(
            string collection,
            string id,
            CancellationToken cancellationToken = default
        )
        {
            return _http.SendAsync<T>(
                HttpMethod.Get,
                "collections/" + Encode(collection)
                    + "/records/" + Encode(id),
                null,
                cancellationToken
            );
        }

        public Task<T> CreateAsync<T>(
            string collection,
            T record,
            CancellationToken cancellationToken = default
        )
        {
            return _http.SendAsync<T>(
                HttpMethod.Post,
                "collections/" + Encode(collection) + "/records",
                record,
                cancellationToken
            );
        }

        public Task<T> UpdateAsync<T>(
            string collection,
            string id,
            T record,
            CancellationToken cancellationToken = default
        )
        {
            return _http.SendAsync<T>(
                new HttpMethod("PATCH"),
                "collections/" + Encode(collection) + "/records/" + Encode(id),
                record,
                cancellationToken
            );
        }

        public async Task DeleteAsync(
            string collection,
            string id,
            CancellationToken cancellationToken = default
        )
        {
            await _http.SendAsync<object>(
                HttpMethod.Delete,
                "collections/" + Encode(collection) + "/records/" + Encode(id),
                null,
                cancellationToken
            );
        }

        private static string Encode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value is required.", "value");
            }

            return Uri.EscapeDataString(value);
        }
    }
}
