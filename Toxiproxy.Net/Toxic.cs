using System.Dynamic;

namespace Toxiproxy.Net
{


    public abstract class Toxic
    {
        public bool Enabled { get; set; }

        public abstract string ToxicType { get; }
    }
}