namespace Toxiproxy.Net
{
    public class LatencyToxic : Toxic
    {
        public int Latency { get; set; }
        public int Jitter { get; set; }

        public override string ToxicType {
            get { return "latency"; }
        }
    }
}