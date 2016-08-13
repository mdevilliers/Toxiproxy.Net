namespace Toxiproxy.Net.Toxics
{
    public class LatencyToxic : ToxicBase
    {
        /// <summary>
        /// The attributes for the Latency Toxic
        /// </summary>
        public class ToxicAttributes
        {
            public int Latency { get; set; }
            public int Jitter { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LatencyToxic"/> class.
        /// </summary>
        public LatencyToxic()
        {
            Attributes = new ToxicAttributes();
        }

        public ToxicAttributes Attributes { get; set; }

        public override string Type
        {
            get { return ToxicTypenames.LatencyToxic; }
        }
    }
}
