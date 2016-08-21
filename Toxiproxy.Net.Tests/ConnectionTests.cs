using System;
using Xunit;

namespace Toxiproxy.Net.Tests
{
    [Collection("Integration")]
    public class ConnectionTests : ToxiproxyTestsBase
    {
        [Fact]
        public void ErrorThrownIfHostIsNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var connection = new Connection("");

            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var connection = new Connection(null);
            });
        }

        [Fact]
        public void DisposeEnablesAndResetsAllProxies()
        {
            var connection = new Connection(resetAllToxicsAndProxiesOnClose: true);

            var client = connection.Client();
            client.Add(ProxyOne);

            var proxy = client.FindProxy(ProxyOne.Name);
            proxy.Enabled = false;
            proxy.Update();

            connection.Dispose();

            var proxyCopy = client.FindProxy(ProxyOne.Name);
            Assert.True(proxyCopy.Enabled);
        }
    }
}