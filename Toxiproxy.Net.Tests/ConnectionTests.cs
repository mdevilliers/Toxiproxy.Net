using System;
using Xunit;

namespace Toxiproxy.Net.Tests
{
    [Collection("Integration")]
    public class ConnectionTests : IDisposable 
    {
        private readonly TestFixture _fixture;

        public ConnectionTests()
        {
           _fixture = new TestFixture(); 
        }
        
        [Fact]
        public void ErrorThrownIfHostIsNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new Connection("");
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new Connection(null);
            });
        }

        [Fact]
        public void DisposeEnablesAndResetsAllProxies()
        {
            var connection = new Connection(resetAllToxicsAndProxiesOnClose: true);

            var client = connection.Client();
            client.Add(_fixture.ProxyOne);

            var proxy = client.FindProxy(_fixture.ProxyOne.Name);
            proxy.Enabled = false;
            proxy.Update();

            connection.Dispose();

            var proxyCopy = client.FindProxy(_fixture.ProxyOne.Name);
            Assert.True(proxyCopy.Enabled);
        }

        public void Dispose()
            => _fixture?.Dispose();
    }
}