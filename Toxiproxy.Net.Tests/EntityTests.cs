using Xunit;

namespace Toxiproxy.Net.Tests
{
    public class EntityTests : ToxiproxyTestsBase
    {
        public EntityTests() : base()
        {
            
        }

        [Fact]
        public void ProxyEntityHasToxics()
        {
            var client = _connection.Client();
            var proxy = client.FindProxy("one");

            Assert.NotNull(proxy.DownStreams());
            Assert.NotNull(proxy.UpStreams());

            Assert.NotNull(proxy.DownStreams().LatencyToxic);
            Assert.NotNull(proxy.DownStreams().SlowCloseToxic);
            Assert.NotNull(proxy.DownStreams().TimeoutToxic);

            Assert.NotNull(proxy.UpStreams().LatencyToxic);
            Assert.NotNull(proxy.UpStreams().SlowCloseToxic);
            Assert.NotNull(proxy.UpStreams().TimeoutToxic);
        }

        [Fact]
        public void UpdatingAProxyWorks()
        {
            var client = _connection.Client();
            var proxy = client.FindProxy("one");

            proxy.Enabled = false;
            proxy.Listen = "127.0.0.1:44355";
            proxy.Upstream = "gmail.com:443";

            proxy.Update();

            var proxy_copy = client.FindProxy("one");

            Assert.Equal(proxy.Enabled, proxy_copy.Enabled);
            Assert.Equal(proxy.Listen, proxy_copy.Listen);
            Assert.Equal(proxy.Upstream, proxy_copy.Upstream);
        }

        [Fact]
        public void UpdatingAndSavingAToxicWorks()
        {
            var client = _connection.Client();
            var latencyToxic = client.FindProxy("one").UpStreams().LatencyToxic;

            latencyToxic.Enabled = false;
            latencyToxic.Jitter = 6666;
            latencyToxic.Latency = 5555;

            latencyToxic.Update();

            var latency_copy = client.FindUpStreamToxicsForProxy(client.FindProxy("one")).LatencyToxic;

            Assert.Equal(latencyToxic.Enabled, latency_copy.Enabled);
            Assert.Equal(latencyToxic.Jitter, latency_copy.Jitter);
            Assert.Equal(latencyToxic.Latency, latency_copy.Latency);

        }
    }
}