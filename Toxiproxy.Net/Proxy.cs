
using RestSharp.Deserializers;

namespace Toxiproxy.Net
{
    public class Proxy
    {
        public string Name { get; set; }
        public string Listen { get; set; }
        public string Upstream { get; set; }
        public bool Enabled { get; set; }

        [DeserializeAs(Name = "upstream_toxics")]
        public ToxicCollection UpStreamToxics { get; set; }

        [DeserializeAs(Name = "downstream_toxics")]
        public ToxicCollection DownStreamToxics { get; set; }
        
    }
}