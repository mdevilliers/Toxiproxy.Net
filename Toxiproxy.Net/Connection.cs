using System;

namespace Toxiproxy.Net
{
    /// <inheritdoc />
    /// <summary>
    /// The class to connect to the ToxiProxy server
    /// </summary>
    /// <seealso cref="T:System.IDisposable" />
    public class Connection : IDisposable
    {
        private const int DefaultListeningPort = 8474;

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

            _resetAllToxicsAndProxiesOnClose = resetAllToxicsAndProxiesOnClose;
            _clientFactory = new HttpClientFactory(new Uri($"http://{host}:{port}/"));
        }

        public Client Client()
            => new Client(_clientFactory);

        public void Dispose()
        {
            if (_resetAllToxicsAndProxiesOnClose)
            {
                Client().Reset();
            }
        }
    }
}
