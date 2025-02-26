namespace GamaEdtech.Common.HttpProvider
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    using GamaEdtech.Common.Core;

    public sealed class HttpProvider(Lazy<IHttpClientFactory> httpClientFactory) : IHttpProvider
    {
        private readonly Lazy<IHttpClientFactory> httpClientFactory = httpClientFactory;

        public IEnumerable<(string Key, string? Value)>? DefaultHeaders { get; set; }

        public async Task<TResponse?> DeleteAsync<TRequest, TResponse, TBody>([NotNull] HttpProviderRequest<TBody, TRequest> request, Func<HttpResponseMessage, Task>? postCallHandler = null, Func<HttpResponseMessage, Task<TResponse?>>? decodeHandler = null, Func<TRequest?, TResponse?, Task<TResponse?>>? failHandler = null)
            where TRequest : IHttpRequest
            where TResponse : IHttpResponse
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            var client = httpClientFactory.Value.CreateHttpClient(request.ForceTls13);
#pragma warning restore CA2000 // Dispose objects before losing scope
            if (request.Timeout.HasValue)
            {
                client.Timeout = TimeSpan.FromMilliseconds(request.Timeout.Value);
            }

            if (!string.IsNullOrEmpty(request.BaseAddress))
            {
                client.BaseAddress = new Uri(request.BaseAddress);
            }

            if (DefaultHeaders is not null)
            {
                foreach (var (key, value) in DefaultHeaders)
                {
                    client.DefaultRequestHeaders.Add(key, value);
                }
            }

            if (request.HeaderParameters?.Count > 0)
            {
                for (var i = 0; i < request.HeaderParameters.Count; i++)
                {
                    client.DefaultRequestHeaders.Add(request.HeaderParameters[i].Key, request.HeaderParameters[i].Value);
                }
            }

            HttpResponseMessage response;

            if (request.Body is null)
            {
                response = await client.DeleteAsync(request.Uri);
            }
            else
            {
                using var req = new HttpRequestMessage(HttpMethod.Delete, request.Uri);
                req.Content = request.Body is List<KeyValuePair<string, string?>> keyValuePairBody
                    ? new FormUrlEncodedContent(keyValuePairBody)
                    : JsonContent.Create(request.Body);

                response = await client.SendAsync(req);
            }

            if (postCallHandler is not null)
            {
                await postCallHandler(response);
            }

            var result = decodeHandler is not null ? await decodeHandler(response) : await response.Content.ReadFromJsonAsync<TResponse>();

            return response.StatusCode is not System.Net.HttpStatusCode.OK && failHandler is not null ? await failHandler(request.Request, result) : result;
        }

        public async Task<TResponse?> GetAsync<TRequest, TResponse, TBody>([NotNull] HttpProviderRequest<TBody, TRequest> request, Func<HttpResponseMessage, Task>? postCallHandler = null, Func<HttpResponseMessage, Task<TResponse?>>? decodeHandler = null, Func<TRequest?, TResponse?, Task<TResponse?>>? failHandler = null)
            where TRequest : IHttpRequest
            where TResponse : IHttpResponse
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            var client = httpClientFactory.Value.CreateHttpClient(request.ForceTls13);
#pragma warning restore CA2000 // Dispose objects before losing scope
            if (request.Timeout.HasValue)
            {
                client.Timeout = TimeSpan.FromMilliseconds(request.Timeout.Value);
            }

            if (!string.IsNullOrEmpty(request.BaseAddress))
            {
                request.Uri = $"{request.BaseAddress.TrimEnd('/')}/{request.Uri}";
            }

            if (DefaultHeaders is not null)
            {
                foreach (var (key, value) in DefaultHeaders)
                {
                    client.DefaultRequestHeaders.Add(key, value);
                }
            }

            if (request.HeaderParameters?.Count > 0)
            {
                for (var i = 0; i < request.HeaderParameters.Count; i++)
                {
                    client.DefaultRequestHeaders.Add(request.HeaderParameters[i].Key, request.HeaderParameters[i].Value);
                }
            }

            if (request.Body is not null)
            {
                request.Uri = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(request.Uri!, Globals.ObjectToDictionary(request.Body)!.Select(t => new KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>(t.Key, t.Value?.ToString())));
            }

            var response = await client.GetAsync(request.Uri);

            if (postCallHandler is not null)
            {
                await postCallHandler(response);
            }

            var result = decodeHandler is not null ? await decodeHandler(response) : await response.Content.ReadFromJsonAsync<TResponse>();

            return response.StatusCode is not System.Net.HttpStatusCode.OK && failHandler is not null ? await failHandler(request.Request, result) : result;
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse, TBody>([NotNull] HttpProviderRequest<TBody, TRequest> request, Func<HttpResponseMessage, Task>? postCallHandler = null, Func<HttpResponseMessage, Task<TResponse?>>? decodeHandler = null, Func<TRequest?, TResponse?, Task<TResponse?>>? failHandler = null)
            where TRequest : IHttpRequest
            where TResponse : IHttpResponse
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            var client = httpClientFactory.Value.CreateHttpClient(request.ForceTls13);
#pragma warning restore CA2000 // Dispose objects before losing scope
            if (request.Timeout.HasValue)
            {
                client.Timeout = TimeSpan.FromMilliseconds(request.Timeout.Value);
            }

            if (!string.IsNullOrEmpty(request.BaseAddress))
            {
                client.BaseAddress = new Uri(request.BaseAddress);
            }

            if (DefaultHeaders is not null)
            {
                foreach (var (key, value) in DefaultHeaders)
                {
                    client.DefaultRequestHeaders.Add(key, value);
                }
            }

            if (request.HeaderParameters?.Count > 0)
            {
                for (var i = 0; i < request.HeaderParameters.Count; i++)
                {
                    client.DefaultRequestHeaders.Add(request.HeaderParameters[i].Key, request.HeaderParameters[i].Value);
                }
            }

            HttpResponseMessage response;
            if (request.Body is List<KeyValuePair<string, string?>> keyValuePairBody)
            {
                using var req = new HttpRequestMessage(HttpMethod.Post, request.Uri) { Content = new FormUrlEncodedContent(keyValuePairBody), };
                response = await client.SendAsync(req);
            }
            else
            {
                response = request.Body is HttpContent httpContent
                    ? await client.PostAsync(request.Uri, httpContent)
                    : await client.PostAsJsonAsync(request.Uri, request.Body);
            }

            if (postCallHandler is not null)
            {
                await postCallHandler(response);
            }

            var result = decodeHandler is not null ? await decodeHandler(response) : await response.Content.ReadFromJsonAsync<TResponse>();

            return response.StatusCode is not System.Net.HttpStatusCode.OK && failHandler is not null ? await failHandler(request.Request, result) : result;
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse, TBody>([NotNull] HttpProviderRequest<TBody, TRequest> request, Func<HttpResponseMessage, Task>? postCallHandler = null, Func<HttpResponseMessage, Task<TResponse?>>? decodeHandler = null, Func<TRequest?, TResponse?, Task<TResponse?>>? failHandler = null)
            where TRequest : IHttpRequest
            where TResponse : IHttpResponse
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            var client = httpClientFactory.Value.CreateHttpClient(request.ForceTls13);
#pragma warning restore CA2000 // Dispose objects before losing scope
            if (request.Timeout.HasValue)
            {
                client.Timeout = TimeSpan.FromMilliseconds(request.Timeout.Value);
            }

            if (!string.IsNullOrEmpty(request.BaseAddress))
            {
                client.BaseAddress = new Uri(request.BaseAddress);
            }

            if (DefaultHeaders is not null)
            {
                foreach (var (key, value) in DefaultHeaders)
                {
                    client.DefaultRequestHeaders.Add(key, value);
                }
            }

            if (request.HeaderParameters?.Count > 0)
            {
                for (var i = 0; i < request.HeaderParameters.Count; i++)
                {
                    client.DefaultRequestHeaders.Add(request.HeaderParameters[i].Key, request.HeaderParameters[i].Value);
                }
            }

            HttpResponseMessage response;

            if (request.Body is List<KeyValuePair<string, string?>> keyValuePairBody)
            {
                using var req = new HttpRequestMessage(HttpMethod.Put, request.Uri) { Content = new FormUrlEncodedContent(keyValuePairBody), };
                response = await client.SendAsync(req);
            }
            else
            {
                response = request.Body is HttpContent httpContent
                    ? await client.PutAsync(request.Uri, httpContent)
                    : await client.PutAsJsonAsync(request.Uri, request.Body);
            }

            if (postCallHandler is not null)
            {
                await postCallHandler(response);
            }

            var result = decodeHandler is not null ? await decodeHandler(response) : await response.Content.ReadFromJsonAsync<TResponse>();

            return response.StatusCode is not System.Net.HttpStatusCode.OK && failHandler is not null ? await failHandler(request.Request, result) : result;
        }
    }
}
