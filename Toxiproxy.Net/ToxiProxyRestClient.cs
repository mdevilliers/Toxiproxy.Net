using RestSharp;
using System.Net;

namespace Toxiproxy.Net
{
    /// <summary>
    /// This is the REST client to interact with the ToxiProxi HTTP API
    /// </summary>
    public class ToxiProxyRestClient
    {
        private readonly IRestClient _client;

        internal IRestRequest GetDefaultRequestWithErrorParsingBehaviour(string path, Method method)
        {
            var request = new RestRequest(path, method)
            {
                OnBeforeDeserialization = resp =>
                {
                    if (resp.StatusCode != HttpStatusCode.OK && resp.StatusCode != HttpStatusCode.Created)
                    {
                        var parsed = SimpleJson.DeserializeObject<ToxiproxiErrorMessage>(resp.Content);
                        throw new ToxiproxiException(parsed.title) { Status = parsed.status };
                    }
                }
            };
            return request;
        }

        internal IRestResponse<T> Execute<T>(IRestRequest request) where T : new()
        {
            return _client.Execute<T>(request);
        }
    }
}
