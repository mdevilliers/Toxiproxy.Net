using System;
using System.Diagnostics;
using System.Threading;

namespace Toxiproxy.Net.Tests
{
    public class TestFixture : IDisposable
    {
        private Process _process;

        public Client Client()
        {
            var exe = @"./toxiproxy-server-2.1.3.darwin-amd64";
            var processInfo = new ProcessStartInfo
            {
                FileName = exe
            };
            _process = new Process
            {
                StartInfo = processInfo,
            };

            _process.Start();
            Thread.Sleep(500);
            return new Connection().Client();
        }

        public void Dispose()
        {
            if (_process.HasExited == false)
                _process.Kill();
        }

        public readonly Proxy ProxyOne = new Proxy
        {
            Name = "one",
            Enabled = true,
            Listen = "127.0.0.1:11111",
            Upstream = "one.com"
        };

        public readonly Proxy ProxyTwo = new Proxy
        {
            Name = "two",
            Enabled = true,
            Listen = "127.0.0.1:22222",
            Upstream = "two.com"
        };

        public readonly Proxy ProxyThree = new Proxy
        {
            Name = "three",
            Enabled = true,
            Listen = "127.0.0.1:33333",
            Upstream = "three.com"
        };
    }

    public class ToxiproxyTestsBase : IDisposable
    {
        protected Connection _connection;
        protected Process _process;

        protected readonly Proxy ProxyOne = new Proxy
        {
            Name = "one",
            Enabled = true,
            Listen = "127.0.0.1:11111",
            Upstream = "one.com"
        };

        protected readonly Proxy ProxyTwo = new Proxy
        {
            Name = "two",
            Enabled = true,
            Listen = "127.0.0.1:22222",
            Upstream = "two.com"
        };

        protected readonly Proxy ProxyThree = new Proxy
        {
            Name = "three",
            Enabled = true,
            Listen = "127.0.0.1:33333",
            Upstream = "three.com"
        };

        public ToxiproxyTestsBase()
        {
/*            var exe = @"./toxiproxy-server-2.1.3.darwin-amd64";
            var processInfo = new ProcessStartInfo
            {
                FileName = exe
            };
            _process = new Process
            {
                StartInfo = processInfo,
            };

            _process.Start();
            Thread.Sleep(500);
            _connection = new Connection();*/
        }

        public void Dispose()
        {
//            if (_process.HasExited == false)
//            {
//                _process.Kill();
//            }
        }
    }
}