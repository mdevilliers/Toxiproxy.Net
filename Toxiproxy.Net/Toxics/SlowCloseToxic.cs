namespace Toxiproxy.Net.Toxics
{
    public class SlowCloseToxic : ToxicBase
    {
        /// <summary>
        /// The attributes for the Slow Close Toxic
        /// </summary>
        public class ToxicAttributes
        {
            public int Delay { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlowCloseToxic"/> class.
        /// </summary>
        public SlowCloseToxic()
        {
            Attributes = new ToxicAttributes();
        }

        public ToxicAttributes Attributes { get; set; }

        public override string Type
        {
            get { return ToxicTypenames.SlowCloseToxic; }
        }
    }
}
