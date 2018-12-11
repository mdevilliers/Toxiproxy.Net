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
            Assert.Throws<ArgumentNullException>(() => { _ = new Connection(""); });

            Assert.Throws<ArgumentNullException>(() => { _ = new Connection(null); });
        }

        [Fact]
        public void DisposeEnablesAndResetsAllProxies()
        {
            _fixture.Client();
            var connection = new Connection(resetAllToxicsAndProxiesOnClose: true);

            var client = connection.Client();
            client.Add(TestProxy.One);

            var proxy = client.FindProxy(TestProxy.One.Name);
            proxy.Enabled = false;
            proxy.Update();

            connection.Dispose();

            var proxyCopy = client.FindProxy(TestProxy.One.Name);
            Assert.True(proxyCopy.Enabled);
        }

        public void Dispose()
            => _fixture?.Dispose();
    }
}