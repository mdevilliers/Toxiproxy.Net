using System;
using Xunit;

namespace Toxiproxy.Net.Tests
{
    [Collection("Integration")]
    public class ClientTests : ToxiproxyTestsBase
    {
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
            Assert.NotNull(downstream.SlicerToxic);
            Assert.NotNull(upstream);
            Assert.NotNull(upstream.LatencyToxic);
            Assert.NotNull(upstream.SlowCloseToxic);
            Assert.NotNull(upstream.TimeoutToxic);
            Assert.NotNull(upstream.SlicerToxic);
        }

        [Fact]
        public void CanUpdateUpStreamToxic()
        {

            var client = _connection.Client();

            var upstream = client.FindUpStreamToxicsForProxy(one);

            upstream.LatencyToxic.Latency = 1;
            upstream.LatencyToxic.Jitter = 2;

            upstream.SlowCloseToxic.Delay = 3;

            upstream.TimeoutToxic.Timeout = 4;

            upstream.BandwidthToxic.Rate = 5;

            upstream.SlicerToxic.Average_Size = 1024;
            upstream.SlicerToxic.Size_Variation = 512;
            upstream.SlicerToxic.Delay = 10;

            client.UpdateUpStreamToxic(one, upstream.LatencyToxic);
            client.UpdateUpStreamToxic(one, upstream.SlowCloseToxic);
            client.UpdateUpStreamToxic(one, upstream.TimeoutToxic);
            client.UpdateUpStreamToxic(one, upstream.BandwidthToxic);
            client.UpdateUpStreamToxic(one, upstream.SlicerToxic);

            var upstreamCopy = client.FindUpStreamToxicsForProxy(one);

            Assert.Equal(upstream.LatencyToxic.Latency, upstreamCopy.LatencyToxic.Latency);
            Assert.Equal(upstream.LatencyToxic.Jitter, upstreamCopy.LatencyToxic.Jitter);

            Assert.Equal(upstream.SlowCloseToxic.Delay, upstreamCopy.SlowCloseToxic.Delay);

            Assert.Equal(upstream.TimeoutToxic.Timeout, upstreamCopy.TimeoutToxic.Timeout);

            Assert.Equal(upstream.BandwidthToxic.Rate, upstreamCopy.BandwidthToxic.Rate);

            Assert.Equal(upstream.SlicerToxic.Average_Size, upstreamCopy.SlicerToxic.Average_Size);
            Assert.Equal(upstream.SlicerToxic.Size_Variation, upstreamCopy.SlicerToxic.Size_Variation);
            Assert.Equal(upstream.SlicerToxic.Delay, upstreamCopy.SlicerToxic.Delay);
            
        }

        [Fact]
        public void CanUpdateDownStreamToxic()
        {
            var client = _connection.Client();

            var downstream = client.FindDownStreamToxicsForProxy(one);

            downstream.LatencyToxic.Latency = 1;
            downstream.LatencyToxic.Jitter = 2;

            downstream.SlowCloseToxic.Delay = 3;

            downstream.TimeoutToxic.Timeout = 4;

            downstream.BandwidthToxic.Rate = 5;

            downstream.SlicerToxic.Average_Size = 1024;
            downstream.SlicerToxic.Size_Variation = 512;
            downstream.SlicerToxic.Delay = 10;

            client.UpdateDownStreamToxic(one, downstream.LatencyToxic);
            client.UpdateDownStreamToxic(one, downstream.SlowCloseToxic);
            client.UpdateDownStreamToxic(one, downstream.TimeoutToxic);
            client.UpdateDownStreamToxic(one, downstream.BandwidthToxic);
            client.UpdateDownStreamToxic(one, downstream.SlicerToxic);

            var downStreamCopy = client.FindDownStreamToxicsForProxy(one);

            Assert.Equal(downstream.LatencyToxic.Latency, downStreamCopy.LatencyToxic.Latency);
            Assert.Equal(downstream.LatencyToxic.Jitter, downStreamCopy.LatencyToxic.Jitter);

            Assert.Equal(downstream.SlowCloseToxic.Delay, downStreamCopy.SlowCloseToxic.Delay);

            Assert.Equal(downstream.TimeoutToxic.Timeout, downStreamCopy.TimeoutToxic.Timeout);

            Assert.Equal(downstream.BandwidthToxic.Rate, downStreamCopy.BandwidthToxic.Rate);

            Assert.Equal(downstream.SlicerToxic.Average_Size, downStreamCopy.SlicerToxic.Average_Size);
            Assert.Equal(downstream.SlicerToxic.Size_Variation, downStreamCopy.SlicerToxic.Size_Variation);
            Assert.Equal(downstream.SlicerToxic.Delay, downStreamCopy.SlicerToxic.Delay);
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

        [Fact]
        public void CanNotAddANullProxy()
        {
            var client = _connection.Client();

            Assert.Throws<ArgumentNullException>(() =>
            {
                client.Add(null);
            });
        }
    }
}
