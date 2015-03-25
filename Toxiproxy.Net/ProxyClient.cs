using System;
using System.Collections.Generic;
using RestSharp;

namespace Toxiproxy.Net
{
    public class ProxyClient
    {
        private readonly IRestClient _client;
        public ProxyClient(IRestClient client)
        {
            this._client = client;
        }

        public IDictionary<string, Proxy> All()
        {
            var request = new RestRequest("/proxies", Method.GET);
            var response = this._client.Execute<Dictionary<string, Proxy>>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            if (response.Data == null)
            {
                return new Dictionary<string, Proxy>();
            }
            Console.WriteLine(response.Content);
            return response.Data;
        }

        public void Add(Proxy proxy)
        {
            var request = new RestRequest("/proxies", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };

            request.AddJsonBody(proxy);

            var response = this._client.Execute(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            Console.WriteLine(response.Content);
        }

        public Proxy Update(Proxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            var request = new RestRequest("/proxies/{name}", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddUrlSegment("name", proxy.Name);
            request.AddJsonBody(proxy);

            var response = this._client.Execute<Proxy>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            //TODO : {"title":"listen tcp: invalid port 78787","status":500} 
            Console.WriteLine(response.Content);
            return response.Data;

        }

        public Proxy FindProxy(string proxyName)
        {
            if (string.IsNullOrEmpty(proxyName))
            {
                throw new ArgumentNullException("proxyName");
            }

            var request = new RestRequest("/proxies/{name}", Method.GET);
            request.AddUrlSegment("name", proxyName);

            var response = this._client.Execute<Proxy>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            Console.WriteLine(response.Content);
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

            var request = new RestRequest("/proxies/{name}/upstream/toxics", Method.GET);
            request.AddUrlSegment("name", proxyName);

            var response = this._client.Execute<ToxicCollection>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            //{"title":"Proxy with name yyy doesn't exist","status":404} 
            Console.WriteLine(response.Content);
           return response.Data;
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

            var request = new RestRequest("/proxies/{name}/downstream/toxics", Method.GET);
            request.AddUrlSegment("name", proxyName);

            var response = this._client.Execute<ToxicCollection>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            //{"title":"Proxy with name yyy doesn't exist","status":404} 
            Console.WriteLine(response.Content);
            return response.Data;
        }

        public void UpdateUpStreamToxic(Proxy proxy, Toxic toxic)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }
            if (toxic == null)
            {
                throw new ArgumentNullException("toxic");
            }
            UpdateUpStreamToxic(proxy.Name, toxic);
        }

        public void UpdateUpStreamToxic(string proxyName, Toxic toxic)
        {
            if (string.IsNullOrEmpty(proxyName))
            {
                throw new ArgumentNullException("proxyName");
            }

            var request = new RestRequest("/proxies/{proxyName}/upstream/toxics/{toxicName}", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddUrlSegment("proxyName", proxyName);
            request.AddUrlSegment("toxicName", toxic.ToxicType);
            request.AddJsonBody(toxic);

            var response = this._client.Execute(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            //TODO : {"title":"listen tcp: invalid port 78787","status":500} 
            Console.WriteLine(response.Content);
        }

        public void UpdateDownStreamToxic(Proxy proxy, Toxic toxic)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }
            if (toxic == null)
            {
                throw new ArgumentNullException("toxic");
            }
            UpdateDownStreamToxic(proxy.Name, toxic);
        }

        public void UpdateDownStreamToxic(string proxyName, Toxic toxic)
        {
            if (string.IsNullOrEmpty(proxyName))
            {
                throw new ArgumentNullException("proxyName");
            }

            var request = new RestRequest("/proxies/{proxyName}/downstream/toxics/{toxicName}", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddUrlSegment("proxyName", proxyName);
            request.AddUrlSegment("toxicName", toxic.ToxicType);
            request.AddJsonBody(toxic);

            var response = this._client.Execute(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            //TODO : {"title":"listen tcp: invalid port 78787","status":500} 
            Console.WriteLine(response.Content);
        }

        public void Delete(Proxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }
            this.Delete(proxy.Name);
        }

        public void Delete(string proxyName)
        {
            if (string.IsNullOrEmpty(proxyName))
            {
                throw new ArgumentNullException("proxyName");
            }

            var request = new RestRequest("/proxies/{name}", Method.DELETE);
            request.AddUrlSegment("name", proxyName);

            var response = this._client.Execute(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            Console.WriteLine(response.Content);
        }

        public void Reset()
        {
            var request = new RestRequest("/reset", Method.GET);
            var response = this._client.Execute(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            } 
            Console.WriteLine(response.Content);
        }
    }
}