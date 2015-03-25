
using System;
using System.Diagnostics;
using Xunit;

namespace Toxiproxy.Net.Tests
{
    public class ProxyClientTests : IDisposable
    {
        private readonly Connection _connection;
        private readonly Process _process;

        private readonly Proxy one = new Proxy() { Name = "one", Enabled = true, Listen = "127.0.0.1:44399", Upstream = "google.com:443" };
        private readonly Proxy two = new Proxy() { Name = "two", Enabled = true, Listen = "127.0.0.1:44377", Upstream = "google.com:443" };

        public ProxyClientTests() 
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

        private void CreateKnownState()
        {
            var client = _connection.Proxies();
            foreach (var proxyName in client.All().Keys)
            {
                client.Delete(proxyName);
            }

            client.Add(one);
            client.Add(two);
        }

        [Fact]
        public void ErrorsAreThrownCorrectly()
        {
            var client = _connection.Proxies();

            Assert.Throws<ToxiproxiException>(() =>
            {
                client.FindProxy("DOESNOTEXIST");

            });

            Assert.Throws<ToxiproxiException>(() =>
            {
                client.FindDownStreamToxicsForProxy("DOESNOTEXIST");

            });

            Assert.Throws<ToxiproxiException>(() =>
            {
                client.FindUpStreamToxicsForProxy("DOESNOTEXIST");

            });
        }

        [Fact]
        public void CanFindNamedProxy()
        {
            var client = _connection.Proxies();
            var proxy = client.FindProxy("one");

            Assert.NotNull(proxy);
            Assert.Equal(proxy.Name, one.Name);
            Assert.Equal(proxy.Name, "one");
        }

        [Fact]
        public void CanFindAllProxies()
        {
            var client = _connection.Proxies();
            var all = client.All();

            Assert.NotNull(all);
            Assert.Equal(2, all.Keys.Count);
        }

        public void Dispose()
        {
            _process.Kill();
        }
    }
}
