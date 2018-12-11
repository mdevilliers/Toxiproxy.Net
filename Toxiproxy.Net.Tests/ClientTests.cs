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
            client.Add(TestProxy.One);

            // Retrieve the proxy
            var proxy = client.FindProxy("one");
            
            // Check if it the correct one
            Assert.NotNull(proxy);
            Assert.Equal(proxy.Name, TestProxy.One.Name);
            Assert.Equal(proxy.Upstream, TestProxy.One.Upstream);
        }

        [Fact]
        public void CanFindAllProxies()
        {
            // Create two proxies and add them to the client
            var client = _fixture.Client();
            client.Add(TestProxy.One);
            client.Add(TestProxy.Two);

            // Retrieve all the proxies
            var all = client.All();

            // Check if there are two proxies
            Assert.Equal(2, all.Keys.Count);
            // Check if contains the correct proxies
            var containsProxyOne = all.Keys.Contains(TestProxy.One.Name);
            Assert.True(containsProxyOne);
            var containsProxyTwo = all.Keys.Contains(TestProxy.Two.Name);
            Assert.True(containsProxyOne);
        }

        [Fact]
        public void CanDeleteProxy()
        {
            // Add three proxies
            var client = _fixture.Client();
            client.Add(TestProxy.One);
            client.Add(TestProxy.Two);
            client.Add(TestProxy.Three);

            // Delete two proxies
            client.Delete(TestProxy.One);
            client.Delete(TestProxy.Two.Name);

            // The client should contain only a proxy
            var all = client.All();
            Assert.Equal(1, all.Keys.Count);

            // The single proxy in the collection should be the 3th proxy
            var containsProxyThree = all.Keys.Contains(TestProxy.Three.Name);
            Assert.True(containsProxyThree);
        }

        [Fact]
        public void CanUpdateProxy()
        {
            // Add a proxy
            var client = _fixture.Client();
            client.Add(TestProxy.One);

            // Retrieve the proxy and update the proxy
            var proxyToUpdate = client.FindProxy(TestProxy.One.Name);
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
            client.Add(TestProxy.One);

            // Reset
            client.Reset();

            // Retrieve the proxy
            var proxyCopy = client.FindProxy(TestProxy.One.Name);

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
            var newProxy = client.Add(TestProxy.One);

            Assert.Equal(TestProxy.One.Name, newProxy.Name);
            Assert.Equal(TestProxy.One.Enabled, newProxy.Enabled);
            Assert.Equal(TestProxy.One.Listen, newProxy.Listen);
            Assert.Equal(TestProxy.One.Upstream, newProxy.Upstream);
        }

        [Fact]
        public void DeletingAProxyMoreThanOnceShouldThrowException()
        {
            // Add a proxy and check it exists
            var client = _fixture.Client();
            client.Add(TestProxy.One);
            var proxy = client.FindProxy(TestProxy.One.Name);

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
            client.Add(TestProxy.One);
            var proxy = client.FindProxy(TestProxy.One.Name);

            // delete
            proxy.Delete();

            // check it doesn't exists
            Assert.Throws<ToxiProxiException>(() =>
            {
                client.FindProxy(TestProxy.One.Name);
            });
        }

		[Fact]
		public void AddToxic_NullFields() {
			// Create a proxy and add the proxy to the client
			var client = _fixture.Client();
			client.Add(TestProxy.One);

			// Retrieve the proxy
			var proxy = client.FindProxy( "one" );
			var latencyToxic = new LatencyToxic
			{
				Attributes = {Latency = 1000}
			};

			proxy.Add( latencyToxic );
			proxy.Update();

			var toxics = proxy.GetAllToxics();
			Assert.True(toxics.Count() == 1);
			var toxic = toxics.First();

			Assert.True(toxic.Toxicity == 1);
			Assert.Equal( ToxicDirection.DownStream, toxic.Stream );

			//default pattern is <type>_<stream>
			Assert.Equal( "latency_downstream", toxic.Name );
		}

		[Fact]
		public void AddToxic_NonNullFields() {
			// Create a proxy and add the proxy to the client
			var client = _fixture.Client();
			client.Add(TestProxy.One);

			// Retrieve the proxy
			var proxy = client.FindProxy( "one" );

			var latencyToxic = new LatencyToxic
			{
				Attributes = {Latency = 1000},
				Stream = ToxicDirection.UpStream,
				Name = "testName",
				Toxicity = 0.5
			};

			proxy.Add( latencyToxic );
			proxy.Update();

			var toxics = proxy.GetAllToxics();
			Assert.True(toxics.Count() == 1);
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
