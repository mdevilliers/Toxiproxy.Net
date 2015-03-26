using Xunit;

namespace Toxiproxy.Net.Tests
{
    public class ClientTests : ToxiproxyTestsBase
    {
        public ClientTests() : base()
        {
        }

        [Fact]
        public void ErrorsAreThrownCorrectly()
        {
            var client = _connection.Client();

            Assert.Throws<ToxiproxiException>(() =>
            {
                client.FindProxy("DOESNOTEXIST");
            });

            Assert.Throws<ToxiproxiException>(() =>
            {
                client.FindDownStreamToxicsForProxy("DOESNOTEXIST");

            });

            Assert.Throws<ToxiproxiException>(() =>
            {
                client.FindUpStreamToxicsForProxy("DOESNOTEXIST");
            });

        }

        [Fact]
        public void CanFindNamedProxy()
        {
            var client = _connection.Client();
            var proxy = client.FindProxy("one");
            
            Assert.NotNull(proxy);
            Assert.Equal(proxy.Name, one.Name);
            Assert.Equal(proxy.Name, "one");
        }

        [Fact]
        public void CanFindAllProxies()
        {
            var client = _connection.Client();
            var all = client.All();

            Assert.NotNull(all);
            Assert.Equal(2, all.Keys.Count);
        }

        [Fact]
        public void CanFindAllToxics()
        {
            var client = _connection.Toxics();
            var all = client.All();

            Assert.NotNull(all);
            Assert.Equal(2, all.Keys.Count);

        }

        [Fact]
        public void CanDeleteProxy()
        {
            var client = _connection.Client();
            client.Delete(one);
            client.Delete(two.Name);

            var all = client.All();
            Assert.NotNull(all);
            Assert.Equal(0, all.Keys.Count);
        }

        [Fact]
        public void CanUpdateProxy()
        {
            var client = _connection.Client();

            one.Enabled = false;
            one.Listen = "localhost:55555";
            one.Upstream = "gmail.com:443";

            client.Update(one);

            var one_copy = client.FindProxy(one.Name);

            Assert.Equal(one.Enabled, one_copy.Enabled);
            Assert.Equal(one.Listen, one_copy.Listen);
            Assert.Equal(one.Upstream, one_copy.Upstream);
        }

        [Fact]
        public void CanFindUpStreamAndDownStreamToxicsForProxy()
        {
            var client = _connection.Client();

            var downstream = client.FindDownStreamToxicsForProxy(one);
            var upstream = client.FindUpStreamToxicsForProxy(one);

            Assert.NotNull(downstream);
            Assert.NotNull(downstream.LatencyToxic);
            Assert.NotNull(downstream.SlowCloseToxic);
            Assert.NotNull(downstream.TimeoutToxic);
            Assert.NotNull(upstream);
            Assert.NotNull(upstream.LatencyToxic);
            Assert.NotNull(upstream.SlowCloseToxic);
            Assert.NotNull(upstream.TimeoutToxic);
        }

        [Fact]
        public void CanUpdateUpStreamToxic()
        {

            var client = _connection.Client();

            var upstream = client.FindUpStreamToxicsForProxy(one);

            upstream.LatencyToxic.Latency = 77777;
            upstream.SlowCloseToxic.Delay = 88888;
            upstream.TimeoutToxic.Timeout = 99999;

            client.UpdateUpStreamToxic(one, upstream.LatencyToxic);
            client.UpdateUpStreamToxic(one, upstream.SlowCloseToxic);
            client.UpdateUpStreamToxic(one, upstream.TimeoutToxic);

            var upstreamCopy = client.FindUpStreamToxicsForProxy(one);

            Assert.Equal(upstream.LatencyToxic.Latency, upstreamCopy.LatencyToxic.Latency);
            Assert.Equal(upstream.SlowCloseToxic.Delay, upstreamCopy.SlowCloseToxic.Delay);
            Assert.Equal(upstream.TimeoutToxic.Timeout, upstreamCopy.TimeoutToxic.Timeout);

        }

        [Fact]
        public void CanUpdateDownStreamToxic()
        {
            var client = _connection.Client();

            var downstream = client.FindDownStreamToxicsForProxy(one);

            downstream.LatencyToxic.Latency = 77777;
            downstream.SlowCloseToxic.Delay = 88888;
            downstream.TimeoutToxic.Timeout = 99999;

            client.UpdateDownStreamToxic(one, downstream.LatencyToxic);
            client.UpdateDownStreamToxic(one, downstream.SlowCloseToxic);
            client.UpdateDownStreamToxic(one, downstream.TimeoutToxic);

            var upstreamCopy = client.FindDownStreamToxicsForProxy(one);

            Assert.Equal(downstream.LatencyToxic.Latency, upstreamCopy.LatencyToxic.Latency);
            Assert.Equal(downstream.SlowCloseToxic.Delay, upstreamCopy.SlowCloseToxic.Delay);
            Assert.Equal(downstream.TimeoutToxic.Timeout, upstreamCopy.TimeoutToxic.Timeout);

        }

        [Fact]
        public void ResetWorks()
        {
            var client = _connection.Client();

            var proxy = client.FindProxy(one.Name);

            proxy.Enabled = false;

            client.Update(proxy);

            client.Reset();

            var proxy_copy = client.FindProxy(one.Name);

            Assert.Equal(proxy_copy.Enabled, true);

        }

    }
}
