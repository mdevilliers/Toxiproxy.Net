using System;
using RestSharp;

namespace Toxiproxy.Net
{
    public class Connection
    {
        private readonly string _host;
        private readonly int _port;
        private readonly IRestClient _client;

        public Connection() : this("localhost")
        {
        }

        public Connection(string host)
            : this(host, 8474)
        {
        }

        public Connection(string host, int port)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException("host");
            }
            this._host = host;
            this._port = port;
            this._client = new RestClient(new Uri(string.Format("http://{0}:{1}", _host, _port)));
        }

        public ProxyClient Proxies()
        {
            return new ProxyClient(this._client);
        }

        public ToxicClient Toxics()
        {
            return new ToxicClient(this._client);
        }
    }
}
