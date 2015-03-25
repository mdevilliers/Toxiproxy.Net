
namespace Toxiproxy.Net
{
    public abstract class Toxic
    {
        public bool Enabled { get; set; }

        public abstract string ToxicType { get; }
    }

    public class LatencyToxic : Toxic
    {
        public int Latency { get; set; }
        public int Jitter { get; set; }

        public override string ToxicType
        {
            get { return "latency"; }
        }
    }

    public class SlowCloseToxic : Toxic
    {
        public int Delay { get; set; }

        public override string ToxicType
        {
            get { return "slow_close"; }
        }
    }

    public class TimeoutToxic : Toxic
    {
        public int Timeout { get; set; }

        public override string ToxicType
        {
            get { return "timeout"; }
        }
    }
}