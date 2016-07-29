using System;
using Xunit;

namespace Toxiproxy.Net.Tests
{
    [Collection("Integration")]
    public class EntityTests : ToxiproxyTestsBase
    {
        [Fact]
        public void UpdatingAProxyWorks()
        {
            // Add a proxy
            var client = _connection.Client();
            client.Add(ProxyOne);
            
            // Retrieve and update a proxy
            var proxy = client.FindProxy(ProxyOne.Name);
            proxy.Enabled = false;
            proxy.Listen = "127.0.0.1:44355";
            var updatedProxy = proxy.Update();

            Assert.Equal(proxy.Enabled, updatedProxy.Enabled);
            Assert.Equal(proxy.Listen, updatedProxy.Listen);
        }

        [Fact]
        public void UpdatingAndSavingAToxicWorks()
        {
            throw new NotImplementedException();

            //var client = _connection.Client();
            //var upLatencyToxic = client.FindProxy("one").UpStreams().LatencyToxic;

            //upLatencyToxic.Enabled = false;
            //upLatencyToxic.Jitter = 6666;
            //upLatencyToxic.Latency = 5555;

            //upLatencyToxic.Update();

            //var upLatency_copy = client.FindUpStreamToxicsForProxy(client.FindProxy("one")).LatencyToxic;

            //Assert.Equal(upLatencyToxic.Enabled, upLatency_copy.Enabled);
            //Assert.Equal(upLatencyToxic.Jitter, upLatency_copy.Jitter);
            //Assert.Equal(upLatencyToxic.Latency, upLatency_copy.Latency);

            //var downlatencyToxic = client.FindProxy("one").DownStreams().LatencyToxic;

            //downlatencyToxic.Enabled = false;
            //downlatencyToxic.Jitter = 6666;
            //downlatencyToxic.Latency = 5555;

            //downlatencyToxic.Update();

            //var downlatencyCopy = client.FindDownStreamToxicsForProxy(client.FindProxy("one")).LatencyToxic;

            //Assert.Equal(downlatencyToxic.Enabled, downlatencyCopy.Enabled);
            //Assert.Equal(downlatencyToxic.Jitter, downlatencyCopy.Jitter);
            //Assert.Equal(downlatencyToxic.Latency, downlatencyCopy.Latency);
        }

        [Fact]
        public void DeletingAnEntityWorks()
        {
            // Add a proxy and check it exists
            var client = _connection.Client();
            client.Add(ProxyOne);
            var proxy = client.FindProxy(ProxyOne.Name);

            // deleting is idemnepotent
            proxy.Delete();
            proxy.Delete();

            Assert.Throws<ToxiproxiException>(() =>
            {
                client.FindProxy(ProxyOne.Name);
            });
        }
    }
}