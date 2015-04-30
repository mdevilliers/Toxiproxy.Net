using System;
using System.Diagnostics;

namespace Toxiproxy.Net.Tests
{
    public class ToxiproxyTestsBase : IDisposable
    {
        protected Connection _connection;
        protected Process _process;
        protected readonly Proxy one = new Proxy() { Name = "one", Enabled = true, Listen = "127.0.0.1:44399", Upstream = "google.com:443" };
        protected readonly Proxy two = new Proxy() { Name = "two", Enabled = true, Listen = "127.0.0.1:44377", Upstream = "google.com:443" };

        public ToxiproxyTestsBase() 
        {
            // TODO : start up version of toxiproxy....
            var processInfo = new ProcessStartInfo()
            {
                FileName = @"..\..\..\compiled\Win64\toxiproxy.exe"
            };
            _process = new Process()
            {
                StartInfo = processInfo
            };
            _process.Start();
            
            _connection = new Connection();

            CreateKnownState();
        }

        protected void CreateKnownState()
        {
            var client = _connection.Client();
            foreach (var proxyName in client.All().Keys)
            {
                client.Delete(proxyName);
            }

            client.Add(one);
            client.Add(two);
        }


        public void Dispose()
        {
            if (_process.HasExited == false)
            {
                _process.Kill();
            }
        }
    }
}