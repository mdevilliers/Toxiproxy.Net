using System;
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

        public Proxies Proxies()
        {
            return new Proxies(this._client);
        }

        public Toxics Toxics()
        {
            return new Toxics(this._client);
        }

    }
}
