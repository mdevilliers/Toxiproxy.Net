namespace Toxiproxy.Net.Tests
{
    internal static class TestProxy
    {
        public static readonly Proxy One = new Proxy
        {
            Name = "one",
            Enabled = true,
            Listen = "127.0.0.1:11111",
            Upstream = "one.com"
        };

        public static readonly Proxy Two = new Proxy
        {
            Name = "two",
            Enabled = true,
            Listen = "127.0.0.1:22222",
            Upstream = "two.com"
        };

        public static readonly Proxy Three = new Proxy
        {
            Name = "three",
            Enabled = true,
            Listen = "127.0.0.1:33333",
            Upstream = "three.com"
        };
    }
}