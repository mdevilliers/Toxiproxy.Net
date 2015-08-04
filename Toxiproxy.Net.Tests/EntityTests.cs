using Xunit;

namespace Toxiproxy.Net.Tests
{
    [Collection("Integration")]
    public class EntityTests : ToxiproxyTestsBase
    {
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

            var updatedProxy = proxy.Update();

            Assert.Equal(proxy.Enabled, updatedProxy.Enabled);
            Assert.Equal(proxy.Listen, updatedProxy.Listen);
            Assert.Equal(proxy.Upstream, updatedProxy.Upstream);

            var proxyCopy = client.FindProxy("one");

            Assert.Equal(proxy.Enabled, proxyCopy.Enabled);
            Assert.Equal(proxy.Listen, proxyCopy.Listen);
            Assert.Equal(proxy.Upstream, proxyCopy.Upstream);
        }

        [Fact]
        public void UpdatingAndSavingAToxicWorks()
        {
            var client = _connection.Client();
            var upLatencyToxic = client.FindProxy("one").UpStreams().LatencyToxic;

            upLatencyToxic.Enabled = false;
            upLatencyToxic.Jitter = 6666;
            upLatencyToxic.Latency = 5555;

            upLatencyToxic.Update();

            var upLatency_copy = client.FindUpStreamToxicsForProxy(client.FindProxy("one")).LatencyToxic;

            Assert.Equal(upLatencyToxic.Enabled, upLatency_copy.Enabled);
            Assert.Equal(upLatencyToxic.Jitter, upLatency_copy.Jitter);
            Assert.Equal(upLatencyToxic.Latency, upLatency_copy.Latency);

            var downlatencyToxic = client.FindProxy("one").DownStreams().LatencyToxic;

            downlatencyToxic.Enabled = false;
            downlatencyToxic.Jitter = 6666;
            downlatencyToxic.Latency = 5555;

            downlatencyToxic.Update();

            var downlatencyCopy = client.FindDownStreamToxicsForProxy(client.FindProxy("one")).LatencyToxic;

            Assert.Equal(downlatencyToxic.Enabled, downlatencyCopy.Enabled);
            Assert.Equal(downlatencyToxic.Jitter, downlatencyCopy.Jitter);
            Assert.Equal(downlatencyToxic.Latency, downlatencyCopy.Latency);
        }

        [Fact]
        public void DeletingAnEntityWorks()
        {
            var client = _connection.Client();
            var proxy = client.FindProxy("one");

            proxy.Delete();

            // deleting is idemnepotent
            proxy.Delete();

            Assert.Throws<ToxiproxiException>(() =>
            {
                client.FindProxy("one");
            });
        }
    }
}