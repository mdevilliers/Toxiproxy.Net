using System;
using Xunit;

namespace Toxiproxy.Net.Tests
{
    public class ConnectionTests
    {
        [Fact]
        public void ErrorThrownIfHostIsNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var connection = new Connection("");

            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var connection = new Connection(null);
            });
        }

    }
}