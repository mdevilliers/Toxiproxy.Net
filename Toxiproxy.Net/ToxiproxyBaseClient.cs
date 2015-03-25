using System.Net;
using RestSharp;

namespace Toxiproxy.Net
{
    public class ToxiproxyBaseClient
    {
        protected IRestRequest GetDefaultRequestWithErrorParsingBehaviour(string path, Method method)
        {
            var request = new RestRequest(path, method)
            {
                OnBeforeDeserialization = resp =>
                {
                    if (resp.StatusCode != HttpStatusCode.OK)
                    {
                        var parsed = SimpleJson.DeserializeObject<ToxiproxiErrorMessage>(resp.Content);
                        throw new ToxiproxiException(parsed.title) { Status = parsed.status };
                    }
                }
            };
            return request;
        }
    }

    internal class ToxiproxiErrorMessage
    {
        public int status { get; set; }
        public string title { get; set; }
    }
}