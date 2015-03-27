using System;
using RestSharp;

namespace Toxiproxy.Net
{
    public class Connection : IDisposable
    {
        private readonly string _host;
        private readonly int _port;
        private readonly IRestClient _restClient;
        private readonly bool _resetAllToxicsAndProxiesOnClose ; 

        public Connection(bool resetAllToxicsAndProxiesOnClose = false)
            : this("localhost", resetAllToxicsAndProxiesOnClose)
        {
        }

        public Connection(string host, bool resetAllToxicsAndProxiesOnClose = false)
            : this(host, 8474, resetAllToxicsAndProxiesOnClose)
        {
        }

        public Connection(string host, int port, bool resetAllToxicsAndProxiesOnClose = false)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException("host");
            }
            this._host = host;
            this._port = port;
            this._resetAllToxicsAndProxiesOnClose = resetAllToxicsAndProxiesOnClose;
            this._restClient = new RestClient(new Uri(string.Format("http://{0}:{1}", _host, _port)));
        }

        public Client Client()
        {
            return new Client(this._restClient);
        }

        public ToxicClient ToxicClient()
        {
            return new ToxicClient(this._restClient);
        }

        public void Dispose()
        {
            if (this._resetAllToxicsAndProxiesOnClose)
            {
                this.Client().Reset();
            }
        }
    }
}
