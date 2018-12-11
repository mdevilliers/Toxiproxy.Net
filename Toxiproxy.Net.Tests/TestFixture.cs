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
            if (_process?.HasExited == false)
                _process.Kill();
        }

    }
}
