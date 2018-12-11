using System;
using System.Net.Http;

namespace Toxiproxy.Net
{
    /// <summary>
    /// The factory class to create preconfigured HttpClient
    /// </summary>
    /// <seealso cref="Toxiproxy.Net.IHttpClientFactory" />
    internal class HttpClientFactory : IHttpClientFactory
    {
        private readonly Uri _baseUrl;

        public HttpClientFactory(Uri baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public Uri BaseUrl
        {
            get { return _baseUrl; }
        }

        public HttpClient Create()
        {
            var client = new HttpClient();
            client.BaseAddress = _baseUrl;
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            return client;
        }
    }

    public interface IHttpClientFactory
    {
        Uri BaseUrl { get; }
        HttpClient Create();
    }
}
