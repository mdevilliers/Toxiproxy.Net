namespace Toxiproxy.Net.Toxics
{
    /// <summary>
    /// The base class for all the toxic classes
    /// </summary>
    public abstract class ToxicBase
    {
        public string Name { get; set; }
        public ToxicDirection? Stream { get; set; }
        public double? Toxicity { get; set; }

        public abstract string Type { get; }

        internal Proxy ParentProxy { get; set; }
        internal Client Client { get; set; }
    }
}