using Newtonsoft.Json;

namespace Toxiproxy.Net.Toxics
{
    public class SlicerToxic : ToxicBase
    {
        /// <summary>
        /// The attributes for the Slicer Toxic
        /// </summary>
        public class ToxicAttributes
        {
            [JsonProperty(PropertyName ="average_size")]
            public int AverageSize { get; set; }

            [JsonProperty(PropertyName = "size_variation")]
            public int SizeVariation { get; set; }

            public int Delay { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlicerToxic"/> class.
        /// </summary>
        public SlicerToxic()
        {
            Attributes = new ToxicAttributes();
        }

        public ToxicAttributes Attributes { get; set; }

        public override string Type
        {
            get { return ToxicTypenames.SlicerToxic; }
        }
    }
}
