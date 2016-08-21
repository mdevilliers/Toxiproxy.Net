using System;

namespace Toxiproxy.Net
{
    /// <summary>
    /// The class to connect to the ToxiProxy server
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class Connection : IDisposable
    {
        private const int DefaultListeningPort = 8474;

        private readonly string _host;
        private readonly int _port;
        
        private readonly IHttpClientFactory _clientFactory;
        private readonly bool _resetAllToxicsAndProxiesOnClose;
        
        public Connection(bool resetAllToxicsAndProxiesOnClose = false)
            : this("localhost", resetAllToxicsAndProxiesOnClose)
        {
            // Nothing here   
        }

        public Connection(string host, bool resetAllToxicsAndProxiesOnClose = false)
            : this(host, DefaultListeningPort, resetAllToxicsAndProxiesOnClose)
        {
            // Nothing here
        }

        public Connection(string host, int port, bool resetAllToxicsAndProxiesOnClose = false)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException("host");
            }
            _host = host;
            _port = port;
            _resetAllToxicsAndProxiesOnClose = resetAllToxicsAndProxiesOnClose;
            _clientFactory = new HttpClientFactory(new Uri(string.Format("http://{0}:{1}/", _host, _port)));
        }

        public Client Client()
        {
            return new Client(_clientFactory);
        }

        public void Dispose()
        {
            if (_resetAllToxicsAndProxiesOnClose)
            {
                Client().Reset();
            }
        }
    }
}
