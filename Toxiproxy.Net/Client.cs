using System;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace Toxiproxy.Net
{
    public class Client : ToxiproxyBaseClient
    {
        private readonly IRestClient _client;
        public Client(IRestClient client)
        {
            _client = client;
        }

        public IDictionary<string, Proxy> All()
        {
            var request = GetDefaultRequestWithErrorParsingBehaviour("/proxies", Method.GET);
            var response =_client.Execute<Dictionary<string, Proxy>>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            if (response.Data == null)
            {
                return new Dictionary<string, Proxy>();
            }

            foreach (var proxy in response.Data.Values)
            {
                proxy.Client = this;
            }

            return response.Data;
        }

        public Proxy Add(Proxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            var request = GetDefaultRequestWithErrorParsingBehaviour("/proxies", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(proxy);

            var response = _client.Execute<Proxy>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            response.Data.Client = this;
            return response.Data;
        }

        public Proxy Update(Proxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            var request = GetDefaultRequestWithErrorParsingBehaviour("/proxies/{name}", Method.POST);
            
            request.RequestFormat = DataFormat.Json;
            request.AddUrlSegment("name", proxy.Name);
            request.AddJsonBody(proxy);

            var response = _client.Execute<Proxy>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            response.Data.Client = this;

            return response.Data;
        }

        public Proxy FindProxy(string proxyName)
        {
            if (string.IsNullOrEmpty(proxyName))
            {
                throw new ArgumentNullException("proxyName");
            }

            var request = GetDefaultRequestWithErrorParsingBehaviour("/proxies/{name}", Method.GET);
            request.AddUrlSegment("name", proxyName);
          
            var response = _client.Execute<Proxy>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            response.Data.Client = this;
            return response.Data;
        }

        private T UpdateToxic<T>(string proxyName, Toxic toxic, ToxicDirection direction)
        {
            if (string.IsNullOrEmpty(proxyName))
            {
                throw new ArgumentNullException("proxyName");
            }

            if (toxic == null)
            {
                throw new ArgumentNullException("toxic");
            }

            var request =
                GetDefaultRequestWithErrorParsingBehaviour("/proxies/{proxyName}/{direction}/toxics/{toxicName}", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddUrlSegment("proxyName", proxyName);
            request.AddUrlSegment("toxicName", toxic.ToxicType);
            request.AddUrlSegment("direction", direction.ToString().ToLower());
            request.AddJsonBody(toxic);
          
            var response = _client.Execute(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            var returnedObject = new JsonDeserializer().Deserialize<T>(response);

            if (!returnedObject.Equals(toxic))
            {
                var message = string.Format("Error updating Toxic : toxic returned {0}", response.Content);
                throw new ToxiproxiException(message);
            }
            return returnedObject;
        }

        public void Delete(Proxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }
            Delete(proxy.Name);
        }

        public void Delete(string proxyName)
        {
            if (string.IsNullOrEmpty(proxyName))
            {
                throw new ArgumentNullException("proxyName");
            }

            var request = GetDefaultRequestWithErrorParsingBehaviour("/proxies/{name}", Method.DELETE);
            request.AddUrlSegment("name", proxyName);

            var response = _client.Execute(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
        }

        public void Reset()
        {
            var request = GetDefaultRequestWithErrorParsingBehaviour("/reset", Method.POST);
            var response = _client.Execute(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            } 
        }
    }
}