using System;
using System.Linq;
using Toxiproxy.Net.Toxics;
using Xunit;

namespace Toxiproxy.Net.Tests
{
    [Collection("Integration")]
    public class ClientTests : IDisposable
    {
	    private readonly TestFixture _fixture;

	    public ClientTests()
	    {
		    _fixture = new TestFixture();
	    }
	    
        [Fact]
        public void ErrorsAreThrownCorrectly()
        {
	        var client = _fixture.Client();

            Assert.Throws<ToxiProxiException>(() =>
            {
                client.FindProxy("DOESNOTEXIST");
            });
        }

        [Fact]
        public void CanFindNamedProxy()
        {
            // Create a proxy and add the proxy to the client
            var client = _fixture.Client();
            client.Add(_fixture.ProxyOne);

            // Retrieve the proxy
            var proxy = client.FindProxy("one");
            
            // Check if it the correct one
            Assert.NotNull(proxy);
            Assert.Equal(proxy.Name, _fixture.ProxyOne.Name);
            Assert.Equal(proxy.Upstream, _fixture.ProxyOne.Upstream);
        }

        [Fact]
        public void CanFindAllProxies()
        {
            // Create two proxies and add them to the client
            var client = _fixture.Client();
            client.Add(_fixture.ProxyOne);
            client.Add(_fixture.ProxyTwo);

            // Retrieve all the proxies
            var all = client.All();

            // Check if there are two proxies
            Assert.Equal(2, all.Keys.Count);
            // Check if contains the correct proxies
            var containsProxyOne = all.Keys.Contains(_fixture.ProxyOne.Name);
            Assert.True(containsProxyOne);
            var containsProxyTwo = all.Keys.Contains(_fixture.ProxyTwo.Name);
            Assert.True(containsProxyOne);
        }

        [Fact]
        public void CanDeleteProxy()
        {
            // Add three proxies
            var client = _fixture.Client();
            client.Add(_fixture.ProxyOne);
            client.Add(_fixture.ProxyTwo);
            client.Add(_fixture.ProxyThree);

            // Delete two proxies
            client.Delete(_fixture.ProxyOne);
            client.Delete(_fixture.ProxyTwo.Name);

            // The client should contain only a proxy
            var all = client.All();
            Assert.Equal(1, all.Keys.Count);

            // The single proxy in the collection should be the 3th proxy
            var containsProxyThree = all.Keys.Contains(_fixture.ProxyThree.Name);
            Assert.True(containsProxyThree);
        }

        [Fact]
        public void CanUpdateProxy()
        {
            // Add a proxy
            var client = _fixture.Client();
            client.Add(_fixture.ProxyOne);

            // Retrieve the proxy and update the proxy
            var proxyToUpdate = client.FindProxy(_fixture.ProxyOne.Name);
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
            var client = _fixture.Client();
            client.Add(_fixture.ProxyOne);

            // Reset
            client.Reset();

            // Retrieve the proxy
            var proxyCopy = client.FindProxy(_fixture.ProxyOne.Name);

            // The proxy should be enabled
            Assert.True(proxyCopy.Enabled);
        }

        [Fact]
        public void CanNotAddANullProxy()
        {
            var client = _fixture.Client();

            Assert.Throws<ArgumentNullException>(() => client.Add(null));
        }

        [Fact]
        public void CreateANewProxyShouldWork()
        {
            var client = _fixture.Client();
            var newProxy = client.Add(_fixture.ProxyOne);

            Assert.Equal(_fixture.ProxyOne.Name, newProxy.Name);
            Assert.Equal(_fixture.ProxyOne.Enabled, newProxy.Enabled);
            Assert.Equal(_fixture.ProxyOne.Listen, newProxy.Listen);
            Assert.Equal(_fixture.ProxyOne.Upstream, newProxy.Upstream);
        }

        [Fact]
        public void DeletingAProxyMoreThanOnceShouldThrowException()
        {
            // Add a proxy and check it exists
            var client = _fixture.Client();
            client.Add(_fixture.ProxyOne);
            var proxy = client.FindProxy(_fixture.ProxyOne.Name);

            // deleting is not idempotent and should throw exception
            proxy.Delete();
            var exception = Assert.Throws<ToxiProxiException>(() => proxy.Delete());
            Assert.Equal("Not found", exception.Message);
        }

        [Fact]
        public void DeletingAProxyWorks()
        {
            // Add a proxy and check it exists
            var client = _fixture.Client();
            client.Add(_fixture.ProxyOne);
            var proxy = client.FindProxy(_fixture.ProxyOne.Name);

            // delete
            proxy.Delete();

            // check it doesn't exists
            Assert.Throws<ToxiProxiException>(() =>
            {
                client.FindProxy(_fixture.ProxyOne.Name);
            });
        }

		[Fact]
		public void AddToxic_NullFields() {
			// Create a proxy and add the proxy to the client
			var client = _fixture.Client();
			client.Add(_fixture.ProxyOne);

			// Retrieve the proxy
			var proxy = client.FindProxy( "one" );
			var latencyToxic = new LatencyToxic();
			latencyToxic.Attributes.Latency = 1000;

			proxy.Add( latencyToxic );
			proxy.Update();

			var toxics = proxy.GetAllToxics();
			Assert.Equal( 1, toxics.Count() );
			var toxic = toxics.First();

			Assert.Equal( 1, toxic.Toxicity );
			Assert.Equal( ToxicDirection.DownStream, toxic.Stream );

			//default pattern is <type>_<stream>
			Assert.Equal( "latency_downstream", toxic.Name );
		}

		[Fact]
		public void AddToxic_NonNullFields() {
			// Create a proxy and add the proxy to the client
			var client = _fixture.Client();
			client.Add(_fixture.ProxyOne);

			// Retrieve the proxy
			var proxy = client.FindProxy( "one" );

			var latencyToxic = new LatencyToxic();
			latencyToxic.Attributes.Latency = 1000;
			latencyToxic.Stream = ToxicDirection.UpStream;
			latencyToxic.Name = "testName";
			latencyToxic.Toxicity = 0.5;

			proxy.Add( latencyToxic );
			proxy.Update();

			var toxics = proxy.GetAllToxics();
			Assert.Equal( 1, toxics.Count() );
			var toxic = toxics.First();

			Assert.Equal( 0.5, toxic.Toxicity);
			Assert.Equal( ToxicDirection.UpStream, toxic.Stream );

			//default pattern is <type>_<stream>
			Assert.Equal( "testName", toxic.Name );
		}

		public void Dispose()
			=> _fixture?.Dispose();
    }
}
