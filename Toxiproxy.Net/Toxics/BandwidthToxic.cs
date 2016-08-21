namespace Toxiproxy.Net.Toxics
{
    public class BandwidthToxic : ToxicBase
    {
        /// <summary>
        /// The attributes for the Bandwidth Toxic
        /// </summary>
        public class ToxicAttributes
        {
            public long Rate { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BandwidthToxic"/> class.
        /// </summary>
        public BandwidthToxic()
        {
            Attributes = new ToxicAttributes();
        }

        public ToxicAttributes Attributes { get; set; }

        public override string Type
        {
            get { return ToxicTypenames.BandwidthToxic; }
        }
    }
}
