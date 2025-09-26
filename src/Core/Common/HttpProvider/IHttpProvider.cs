namespace GamaEdtech.Common.HttpProvider
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Threading.Tasks;

    using GamaEdtech.Common.DataAnnotation;

    [Injectable]
    public interface IHttpProvider
    {
        IEnumerable<(string Key, string? Value)>? DefaultHeaders { get; set; }

        Task<TResponse?> PostAsync<TRequest, TResponse, TBody>(HttpProviderRequest<TBody, TRequest> request, Func<HttpResponseMessage, Task>? postCallHandler = null, Func<HttpResponseMessage, Task<TResponse?>>? decodeHandler = null, Func<TRequest?, TResponse?, Task<TResponse?>>? failHandler = null)
            where TRequest : IHttpRequest
            where TResponse : IHttpResponse;

        Task<TResponse?> PutAsync<TRequest, TResponse, TBody>(HttpProviderRequest<TBody, TRequest> request, Func<HttpResponseMessage, Task>? postCallHandler = null, Func<HttpResponseMessage, Task<TResponse?>>? decodeHandler = null, Func<TRequest?, TResponse?, Task<TResponse?>>? failHandler = null)
            where TRequest : IHttpRequest
            where TResponse : IHttpResponse;

        Task<TResponse?> GetAsync<TRequest, TResponse, TBody>(HttpProviderRequest<TBody, TRequest> request, Func<HttpResponseMessage, Task>? postCallHandler = null, Func<HttpResponseMessage, Task<TResponse?>>? decodeHandler = null, Func<TRequest?, TResponse?, Task<TResponse?>>? failHandler = null)
            where TRequest : IHttpRequest
            where TResponse : IHttpResponse;

        Task<byte[]?> GetByteArrayAsync<TRequest, TBody>([NotNull] HttpProviderRequest<TBody, TRequest> request, Func<HttpResponseMessage, Task>? postCallHandler = null, Func<HttpResponseMessage, Task<byte[]?>>? decodeHandler = null, Func<TRequest?, byte[]?, Task<byte[]?>>? failHandler = null)
            where TRequest : IHttpRequest;

        Task<TResponse?> DeleteAsync<TRequest, TResponse, TBody>(HttpProviderRequest<TBody, TRequest> request, Func<HttpResponseMessage, Task>? postCallHandler = null, Func<HttpResponseMessage, Task<TResponse?>>? decodeHandler = null, Func<TRequest?, TResponse?, Task<TResponse?>>? failHandler = null)
            where TRequest : IHttpRequest
            where TResponse : IHttpResponse;
    }
}
