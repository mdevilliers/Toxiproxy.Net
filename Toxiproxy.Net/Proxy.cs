
namespace Toxiproxy.Net
{
    public class Proxy
    {
        public string Name { get; set; }
        public string Listen { get; set; }
        public string Upstream { get; set; }
        public bool Enabled { get; set; }
    }
}