using System;
using Xunit;

namespace Toxiproxy.Net.Tests
{
    [Collection("Integration")]
    public class ClientTests : ToxiproxyTestsBase
    {
        [Fact]
        public void ErrorsAreThrownCorrectly()
        {
            var client = _connection.Client();

            Assert.Throws<ToxiproxiException>(() =>
            {
                client.FindProxy("DOESNOTEXIST");
            });
        }

        [Fact]
        public void CanFindNamedProxy()
        {
            // Create a proxy and add the proxy to the client
            var client = _connection.Client();
            client.Add(ProxyOne);

            // Retrieve the proxy
            var proxy = client.FindProxy("one");
            
            // Check if it the corerct one
            Assert.NotNull(proxy);
            Assert.Equal(proxy.Name, ProxyOne.Name);
            Assert.Equal(proxy.Upstream, ProxyOne.Upstream);
        }

        [Fact]
        public void CanFindAllProxies()
        {
            // Create two proxies and add them to the client
            var client = _connection.Client();
            client.Add(ProxyOne);
            client.Add(ProxyTwo);

            // Retrieve all the proxies
            var all = client.All();

            // Check if there are two proxies
            Assert.Equal(2, all.Keys.Count);
            // Check if contains the correct proxies
            var containsProxyOne = all.Keys.Contains(ProxyOne.Name);
            Assert.True(containsProxyOne);
            var containsProxyTwo = all.Keys.Contains(ProxyTwo.Name);
            Assert.True(containsProxyOne);
        }

        [Fact]
        public void CanDeleteProxy()
        {
            // Add three proxies
            var client = _connection.Client();
            client.Add(ProxyOne);
            client.Add(ProxyTwo);
            client.Add(ProxyThree);

            // Delete two proxies
            client.Delete(ProxyOne);
            client.Delete(ProxyTwo.Name);

            // The client should contain only a proxy
            var all = client.All();
            Assert.Equal(1, all.Keys.Count);

            // The single proxy in the collection should be the 3th proxy
            var containsProxyThree = all.Keys.Contains(ProxyThree.Name);
            Assert.True(containsProxyThree);
        }

        [Fact]
        public void CanUpdateProxy()
        {
            // Add a proxy
            var client = _connection.Client();
            client.Add(ProxyOne);

            // Retrieve the proxy and update the proxy
            var proxyToUpdate = client.FindProxy(ProxyOne.Name);
            proxyToUpdate.Enabled = false;
            proxyToUpdate.Listen = "localhost:55555";
            proxyToUpdate.Upstream = "google.com";
            client.Update(proxyToUpdate);

            // Retrieve the proxy and check if the parameters are correctly updated
            var proxyUpdated = client.FindProxy(proxyToUpdate.Name);

            Assert.Equal(proxyToUpdate.Enabled, proxyUpdated.Enabled);
            Assert.Equal(proxyToUpdate.Listen, proxyUpdated.Listen);
            Assert.Equal(proxyToUpdate.Upstream, proxyUpdated.Upstream);
        }

        [Fact]
        public void ResetWorks()
        {
            // Add a disabled proxy
            var client = _connection.Client();
            client.Add(ProxyOne);

            // Reset
            client.Reset();

            // Retrive the proxy
            var proxyCopy = client.FindProxy(ProxyOne.Name);

            // The proxy should be enabled
            Assert.Equal(proxyCopy.Enabled, true);
        }

        [Fact]
        public void CanNotAddANullProxy()
        {
            var client = _connection.Client();

            Assert.Throws<ArgumentNullException>(() => client.Add(null));
        }

        //------------- NEW TESTS ----------------
        [Fact]
        public void CreateANewProxyShouldWork()
        {
            var client = _connection.Client();
            var newProxy = client.Add(ProxyOne);

            Assert.Equal(ProxyOne.Name, newProxy.Name);
            Assert.Equal(ProxyOne.Enabled, newProxy.Enabled);
            Assert.Equal(ProxyOne.Listen, newProxy.Listen);
            Assert.Equal(ProxyOne.Upstream, newProxy.Upstream);
        }

        [Fact]
        public void ListActiveToxicsShouldWork()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CreateANewLatencyToxicShouldWork()
        {
            var client = _connection.Client();

            var proxy = new Proxy
            {
                Name = "testingProxy",
                Enabled = true,
                Listen = "127.0.0.1:9090",
                Upstream = "google.com"
            };

            var newProxy = client.Add(proxy);

            var toxic = new LatencyToxic {
                Name = "LatencyToxicTest",
                Stream = ToxicDirection.UpStream,
                Enabled = true,
                Jitter = 10,
                Latency = 5
            };
            var newToxic = proxy.Add(toxic);

        }

        [Fact]
        public void CreateANewSlowCloseToxicShouldWork()
        {
            throw new NotImplementedException();

            var client = _connection.Client();

            var proxy = new Proxy
            {
                Name = "testingProxy",
                Enabled = true,
                Listen = "127.0.0.1:9090"
            };

            var newProxy = client.Add(proxy);

            //var toxic = new SlowCloseToxic
            //{
            //    Name = "SlowCloseToxicTest",
            //    Stream = ToxicDirection.UpStream,
            //    Enabled = true,
            //    Jitter = 10,
            //    Latency = 5
            //};
            //var newToxic = proxy.Add(toxic);

        }

        [Fact]
        public void CreateANewTimeoutToxicShouldWork()
        {
            throw new NotImplementedException();

            var client = _connection.Client();

            var proxy = new Proxy
            {
                Name = "testingProxy",
                Enabled = true,
                Listen = "127.0.0.1:9090"
            };

            var newProxy = client.Add(proxy);

            //var toxic = new TimeoutToxic
            //{
            //    Name = "TimeoutToxicTest",
            //    Stream = ToxicDirection.UpStream,
            //    Enabled = true,
            //    Jitter = 10,
            //    Latency = 5
            //};
            //var newToxic = proxy.Add(toxic);

        }

        [Fact]
        public void CreateANewBandwidthToxicShouldWork()
        {
            var client = _connection.Client();
            var proxy = new Proxy
            {
                Name = "testingProxy",
                Enabled = true,
                Listen = "127.0.0.1:9090",
                Upstream = "google.com"
            };

            var newProxy = client.Add(proxy);

            var toxic = new BandwidthToxic
            {
                Name = "BandwidthToxicTest",
                Stream = ToxicDirection.UpStream,
                Enabled = true,
                Rate = 100
            };
            var newToxic = proxy.Add(toxic);
        }

        [Fact]
        public void CreateANewSlicerToxicShouldWork()
        {
            throw new NotImplementedException();

            var client = _connection.Client();

            var proxy = new Proxy
            {
                Name = "testingProxy",
                Enabled = true,
                Listen = "127.0.0.1:9090"
            };

            var newProxy = client.Add(proxy);

            var toxic = new SlicerToxic
            {
                Name = "SlicerToxicTest",
                Stream = ToxicDirection.UpStream,
                Enabled = true,
                Average_Size = 10,
                Delay = 5,
                Size_Variation = 1
            };
            var newToxic = proxy.Add(toxic);

            Assert.Equal(toxic.Name, newToxic.Name);
            Assert.Equal(toxic.Stream, newToxic.Stream);
            Assert.Equal(toxic.Enabled, newToxic.Enabled);
            Assert.Equal(toxic.Average_Size, newToxic.Average_Size);
            Assert.Equal(toxic.Delay, newToxic.Delay);
            Assert.Equal(toxic.Size_Variation, newToxic.Size_Variation);
        }

        [Fact]
        public void GetAToxicShouldWork()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void UpdateAToxicShouldWork()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void DeleteAToxicShouldWork()
        {
            throw new NotImplementedException();
        }
    }
}
