using System;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Deserializers;

namespace Toxiproxy.Net
{
    public class Client : ToxiproxyBaseClient
    {
        private readonly IRestClient _client;
        public Client(IRestClient client)
        {
            this._client = client;
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

        public ToxicCollection FindUpStreamToxicsForProxy(Proxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            return FindUpStreamToxicsForProxy(proxy.Name);
        }

        public ToxicCollection FindUpStreamToxicsForProxy(string proxyName)
        {
            if (string.IsNullOrEmpty(proxyName))
            {
                throw new ArgumentNullException("proxyName");
            }

            var request = GetDefaultRequestWithErrorParsingBehaviour("/proxies/{name}/upstream/toxics", Method.GET);
            request.AddUrlSegment("name", proxyName);

            var response = _client.Execute<ToxicCollection>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            var collection = response.Data;
            InitiliseToxicCollection(collection, ToxicDirection.UpStream, proxyName);

            return collection;
        }

        public ToxicCollection FindDownStreamToxicsForProxy(Proxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            return FindDownStreamToxicsForProxy(proxy.Name);
        }

        public ToxicCollection FindDownStreamToxicsForProxy(string proxyName)
        {
            if (string.IsNullOrEmpty(proxyName))
            {
                throw new ArgumentNullException("proxyName");
            }

            var request = GetDefaultRequestWithErrorParsingBehaviour("/proxies/{name}/downstream/toxics", Method.GET);
            request.AddUrlSegment("name", proxyName);

            var response = _client.Execute<ToxicCollection>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            var collection = response.Data;
            InitiliseToxicCollection(collection, ToxicDirection.DownStream, proxyName);

            return collection;
        }

        public T UpdateUpStreamToxic<T>(Proxy proxy, Toxic<T> toxic)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            return UpdateUpStreamToxic(proxy.Name, toxic);
        }

        public T UpdateUpStreamToxic<T>(string proxyName, Toxic<T> toxic)
        {
            return UpdateToxic(proxyName, toxic, ToxicDirection.UpStream);
        }

        public T UpdateDownStreamToxic<T>(Proxy proxy, Toxic<T> toxic)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            return UpdateDownStreamToxic(proxy.Name, toxic);
        }

        public T UpdateDownStreamToxic<T>(string proxyName, Toxic<T> toxic)
        {
            return UpdateToxic(proxyName, toxic, ToxicDirection.DownStream);
        }

        private T UpdateToxic<T>(string proxyName, Toxic<T> toxic, ToxicDirection direction)
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
            var request = GetDefaultRequestWithErrorParsingBehaviour("/reset", Method.GET);
            var response = _client.Execute(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            } 
        }

        private void InitiliseToxicCollection(ToxicCollection collection, ToxicDirection direction, string proxyName)
        {
            collection.LatencyToxic.Client = this;
            collection.LatencyToxic.Direction = direction;
            collection.LatencyToxic.ParentProxy = proxyName;

            collection.SlowCloseToxic.Client = this;
            collection.SlowCloseToxic.Direction = direction;
            collection.SlowCloseToxic.ParentProxy = proxyName;

            collection.TimeoutToxic.Client = this;
            collection.TimeoutToxic.Direction = direction;
            collection.TimeoutToxic.ParentProxy = proxyName;

            collection.SlicerToxic.Client = this;
            collection.SlicerToxic.Direction = direction;
            collection.SlicerToxic.ParentProxy = proxyName;
        }
    }
}