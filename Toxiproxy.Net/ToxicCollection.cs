using RestSharp.Deserializers;

namespace Toxiproxy.Net
{
    public class ToxicCollection
    { 
        [DeserializeAs(Name = "latency")]
        public LatencyToxic LatencyToxic { get; set; }
        
        [DeserializeAs(Name = "slow_close")]
        public SlowCloseToxic SlowCloseToxic { get; set; }

        [DeserializeAs(Name = "timeout")]
        public TimeoutToxic TimeoutToxic { get; set; }
    }
}