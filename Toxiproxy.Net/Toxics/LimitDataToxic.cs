namespace Toxiproxy.Net.Toxics
{
    public class LimitDataToxic : ToxicBase
    {
        /// <summary>
        /// The attributes for the LimitData Toxic
        /// </summary>
        public class ToxicAttributes
        {
            public long Bytes { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitDataToxic"/> class.
        /// </summary>
        public LimitDataToxic()
        {
            Attributes = new ToxicAttributes();
        }

        public ToxicAttributes Attributes { get; set; }

        public override string Type
        {
            get { return ToxicTypenames.LimitDataToxic; }
        }
    }
}
