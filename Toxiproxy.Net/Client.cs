using System;
using System.Collections.Generic;
using RestSharp;

namespace Toxiproxy.Net
{
    public class Client
    {
        private readonly string _host;
        private readonly int _port;
        private readonly RestClient _client;

        public Client() : this("localhost", 8474)
        {
        }

        public Client(string host)
            : this(host, 8474)
        {
        }

        public Client(string host, int port)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException("host");
            }
            this._host = host;
            this._port = port;
            this._client = new RestClient(new Uri(string.Format("http://{0}:{1}", _host, _port)));
        }

        public IDictionary<string,Proxy> Proxies()
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
           
            return response.Data;
        }

        public void AddProxy(Proxy proxy)
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
        }

        public void DeleteProxy(Proxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }
            this.DeleteProxy(proxy.Name);
        }

        public void DeleteProxy(string proxyname)
        {
            if (string.IsNullOrEmpty(proxyname))
            {
                throw new ArgumentNullException("proxyname");
            }

            var request = new RestRequest("/proxies/{name}", Method.DELETE);
            request.AddUrlSegment("name", proxyname);

            var response = this._client.Execute(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
        }

    }
}
