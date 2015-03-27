using System;
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

            var downlatency_copy = client.FindDownStreamToxicsForProxy(client.FindProxy("one")).LatencyToxic;

            Assert.Equal(downlatencyToxic.Enabled, downlatency_copy.Enabled);
            Assert.Equal(downlatencyToxic.Jitter, downlatency_copy.Jitter);
            Assert.Equal(downlatencyToxic.Latency, downlatency_copy.Latency);
        }

        [Fact]
        public void DeletingAnEntityWorks()
        {
            var client = _connection.Client();
            var proxy = client.FindProxy("one");

            proxy.Delete();

            Assert.Throws<ToxiproxiException>(() =>
            {
                client.FindProxy("one");
            });
        }
    }
}