using System.Linq;
using Toxiproxy.Net.Toxics;
using Xunit;

namespace Toxiproxy.Net.Tests
{
    [Collection("Integration")]
    public class ProxyTests : ToxiproxyTestsBase
    {
        [Fact]
        public void GetAllToxicsFromAProxyShouldWork()
        {
            // Add two toxics to a proxy and check if they are present in the list
            // of the toxies for the given proxy
            var client = _connection.Client();
            var proxy = new Proxy
            {
                Name = "testingProxy",
                Enabled = true,
                Listen = "127.0.0.1:9090",
                Upstream = "google.com"
            };

            var newProxy = client.Add(proxy);

            var slicerToxic = new SlicerToxic
            {
                Name = "SlicerToxicTest",
                Stream = ToxicDirection.UpStream
            };
            slicerToxic.Attributes.AverageSize = 10;
            slicerToxic.Attributes.Delay = 5;
            slicerToxic.Attributes.SizeVariation = 1;
            newProxy.Add(slicerToxic);

            var slowCloseToxic = new SlowCloseToxic
            {
                Name = "slowCloseToxic",
                Stream = ToxicDirection.DownStream,
                Toxicity = 80
            };
            slowCloseToxic.Attributes.Delay = 50;
            newProxy.Add(slowCloseToxic);

            // Retrieve the proxy and check the toxics
            var toxics = newProxy.GetAllToxics();
            Assert.Equal(2, toxics.Count());

            var slicerToxicInTheProxy = toxics.OfType<SlicerToxic>().Single();
            Assert.Equal(slicerToxic.Name, slicerToxicInTheProxy.Name);
            Assert.Equal(slicerToxic.Stream, slicerToxicInTheProxy.Stream);
            Assert.Equal(slicerToxic.Attributes.AverageSize, slicerToxicInTheProxy.Attributes.AverageSize);
            Assert.Equal(slicerToxic.Attributes.Delay, slicerToxicInTheProxy.Attributes.Delay);
            Assert.Equal(slicerToxic.Attributes.SizeVariation, slicerToxicInTheProxy.Attributes.SizeVariation);

            var slowCloseToxicInTheProxy = toxics.OfType<SlowCloseToxic>().Single();
            Assert.Equal(slowCloseToxic.Name, slowCloseToxicInTheProxy.Name);
            Assert.Equal(slowCloseToxic.Stream, slowCloseToxicInTheProxy.Stream);
            Assert.Equal(slowCloseToxic.Attributes.Delay, slowCloseToxicInTheProxy.Attributes.Delay);
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

            var toxic = new LatencyToxic
            {
                Name = "LatencyToxicTest",
                Stream = ToxicDirection.UpStream
            };
            toxic.Attributes.Jitter = 10;
            toxic.Attributes.Latency = 5;
            var newToxic = newProxy.Add(toxic);

            // Need to retrieve the proxy and check the toxic's values
            Assert.Equal(toxic.Name, newToxic.Name);
            Assert.Equal(toxic.Stream, newToxic.Stream);
            Assert.Equal(toxic.Attributes.Jitter, newToxic.Attributes.Jitter);
            Assert.Equal(toxic.Attributes.Latency, newToxic.Attributes.Latency);
        }

        [Fact]
        public void CreateANewSlowCloseToxicShouldWork()
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

            var toxic = new SlowCloseToxic
            {
                Name = "SlowCloseToxicTest",
                Stream = ToxicDirection.UpStream,
                Toxicity = 0.5
            };
            toxic.Attributes.Delay = 100;
            var newToxic = newProxy.Add(toxic);

            // Need to retrieve the proxy and check the toxic's values
            Assert.Equal(toxic.Name, newToxic.Name);
            Assert.Equal(toxic.Stream, newToxic.Stream);
            Assert.Equal(toxic.Attributes.Delay, newToxic.Attributes.Delay);
        }

        [Fact]
        public void CreateANewTimeoutToxicShouldWork()
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

            var toxic = new TimeoutToxic
            {
                Name = "TimeoutToxicTest",
                Stream = ToxicDirection.UpStream,
                Toxicity = 0.5
            };
            toxic.Attributes.Timeout = 10;
            var newToxic = newProxy.Add(toxic);

            // Need to retrieve the proxy and check the toxic's values
            Assert.Equal(toxic.Name, newToxic.Name);
            Assert.Equal(toxic.Stream, newToxic.Stream);
            Assert.Equal(toxic.Attributes.Timeout, newToxic.Attributes.Timeout);
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
                Stream = ToxicDirection.UpStream
            };
            toxic.Attributes.Rate = 100;
            var newToxic = newProxy.Add(toxic);

            // Need to retrieve the proxy and check the toxic's values
            Assert.Equal(toxic.Name, newToxic.Name);
            Assert.Equal(toxic.Stream, newToxic.Stream);
            Assert.Equal(toxic.Attributes.Rate, newToxic.Attributes.Rate);
        }

        [Fact]
        public void CreateANewSlicerToxicShouldWork()
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

            var toxic = new SlicerToxic
            {
                Name = "SlicerToxicTest",
                Stream = ToxicDirection.UpStream
            };
            toxic.Attributes.AverageSize = 10;
            toxic.Attributes.Delay = 5;
            toxic.Attributes.SizeVariation = 1;
            var newToxic = newProxy.Add(toxic);

            // Need to retrieve the proxy and check the toxic's values
            Assert.Equal(toxic.Name, newToxic.Name);
            Assert.Equal(toxic.Stream, newToxic.Stream);
            Assert.Equal(toxic.Attributes.AverageSize, newToxic.Attributes.AverageSize);
            Assert.Equal(toxic.Attributes.Delay, newToxic.Attributes.Delay);
            Assert.Equal(toxic.Attributes.SizeVariation, newToxic.Attributes.SizeVariation);
        }

        [Fact]
        public void AddTwoToxicWithTheSameNameShouldThrowException()
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

            var firstToxic = new SlicerToxic
            {
                Name = "SlicerToxicTest",
                Stream = ToxicDirection.UpStream
            };
            firstToxic.Attributes.AverageSize = 10;
            firstToxic.Attributes.Delay = 5;
            firstToxic.Attributes.SizeVariation = 1;
            newProxy.Add(firstToxic);

            var toxicWithSameName = new SlicerToxic
            {
                Name = firstToxic.Name,
                Stream = ToxicDirection.UpStream
            };

            Assert.Throws<ToxiProxiException>(() => newProxy.Add(toxicWithSameName));
        }

        [Fact]
        public void GetAnExistingToxicFromAProxyShouldWork()
        {
            // Add a toxics to a proxy.
            // After reload the toxic again and check that all the properties
            // are correctly saved 
            var client = _connection.Client();
            var proxy = new Proxy
            {
                Name = "testingProxy",
                Enabled = true,
                Listen = "127.0.0.1:9090",
                Upstream = "google.com"
            };

            proxy = client.Add(proxy);

            var toxic = new SlicerToxic
            {
                Name = "SlicerToxicTest",
                Stream = ToxicDirection.UpStream
            };
            toxic.Attributes.AverageSize = 10;
            toxic.Attributes.Delay = 5;
            toxic.Attributes.SizeVariation = 1;
            toxic = proxy.Add(toxic);

            // Reload the toxic and update the properties
            var toxicInProxy = proxy.GetToxicByName(toxic.Name);

            // Assert
            Assert.Equal(toxicInProxy.Name, toxic.Name);
            Assert.Equal(toxicInProxy.Stream, toxic.Stream);
            Assert.IsType<SlicerToxic>(toxicInProxy);
            var specificToxicInProxy = (SlicerToxic)toxicInProxy;
            Assert.Equal(specificToxicInProxy.Attributes.AverageSize, toxic.Attributes.AverageSize);
            Assert.Equal(specificToxicInProxy.Attributes.Delay, toxic.Attributes.Delay);
            Assert.Equal(specificToxicInProxy.Attributes.SizeVariation, toxic.Attributes.SizeVariation);
        }

        [Fact]
        public void DeleteAToxicShouldWork()
        {
            // Add two toxics to a proxy.
            // After delete the first one and check that
            // there is still the second toxic in the proxy
            var client = _connection.Client();
            var proxy = new Proxy
            {
                Name = "testingProxy",
                Enabled = true,
                Listen = "127.0.0.1:9090",
                Upstream = "google.com"
            };

            var newProxy = client.Add(proxy);

            var firstToxic = new SlicerToxic
            {
                Name = "SlicerToxicTest",
                Stream = ToxicDirection.UpStream
            };
            firstToxic.Attributes.AverageSize = 10;
            firstToxic.Attributes.Delay = 5;
            firstToxic.Attributes.SizeVariation = 1;
            newProxy.Add(firstToxic);

            var secondToxic = new SlowCloseToxic
            {
                Name = "slowCloseToxic",
                Stream = ToxicDirection.DownStream,
                Toxicity = 80
            };
            secondToxic.Attributes.Delay = 50;
            newProxy.Add(secondToxic);

            // Delete the first toxic
            newProxy.RemoveToxic(firstToxic.Name);

            // Retrieve the proxy and check that there is the
            // correct toxics
            var toxicsInProxy = newProxy.GetAllToxics();
            Assert.Equal(1, toxicsInProxy.Count());
            Assert.IsType<SlowCloseToxic>(toxicsInProxy.First());
            var singleToxicInProxy = (SlowCloseToxic)toxicsInProxy.First();
            Assert.Equal(secondToxic.Name, singleToxicInProxy.Name);
            Assert.Equal(secondToxic.Stream, singleToxicInProxy.Stream);
            Assert.Equal(secondToxic.Toxicity, singleToxicInProxy.Toxicity);
            Assert.Equal(secondToxic.Attributes.Delay, singleToxicInProxy.Attributes.Delay);
        }

        [Fact]
        public void UpdatingAToxicShouldWorks()
        {
            // Add a toxics to a proxy.
            // After update all the toxic's properties
            // Reload the toxic again and check that all the properties
            // are correctly updated 
            var client = _connection.Client();
            var proxy = new Proxy
            {
                Name = "testingProxy",
                Enabled = true,
                Listen = "127.0.0.1:9090",
                Upstream = "google.com"
            };

            proxy = client.Add(proxy);

            var toxic = new SlicerToxic
            {
                Name = "SlicerToxicTest",
                Stream = ToxicDirection.UpStream
            };
            toxic.Attributes.AverageSize = 10;
            toxic.Attributes.Delay = 5;
            toxic.Attributes.SizeVariation = 1;
            toxic = proxy.Add(toxic);

            // Reload the toxic and update the properties
            var toxicInProxy = (SlicerToxic)proxy.GetToxicByName(toxic.Name);

            // Update the toxic's property
            toxicInProxy.Name = "NewName";
            toxicInProxy.Stream = ToxicDirection.DownStream;
            toxicInProxy.Attributes.AverageSize = 20;
            toxicInProxy.Attributes.Delay = 10;
            toxicInProxy.Attributes.SizeVariation = 2;
            proxy.UpdateToxic<SlicerToxic>(toxic.Name, toxicInProxy);

            // Reload again
            var updatedToxic = proxy.GetToxicByName(toxicInProxy.Name);

            // Assert
            // WARNING: By design it's not possible to update the name and the stream properties of the proxy.
            Assert.NotEqual(toxicInProxy.Name, updatedToxic.Name);
            Assert.NotEqual(toxicInProxy.Stream, updatedToxic.Stream);
            Assert.IsType<SlicerToxic>(toxicInProxy);
            var specificToxicInProxy = (SlicerToxic)toxicInProxy;
            Assert.Equal(toxicInProxy.Attributes.AverageSize, specificToxicInProxy.Attributes.AverageSize);
            Assert.Equal(toxicInProxy.Attributes.Delay, specificToxicInProxy.Attributes.Delay);
            Assert.Equal(toxicInProxy.Attributes.SizeVariation, specificToxicInProxy.Attributes.SizeVariation);
        }
    }
}
