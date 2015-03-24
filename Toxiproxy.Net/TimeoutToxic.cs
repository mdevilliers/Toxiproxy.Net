namespace Toxiproxy.Net
{
    public class TimeoutToxic : Toxic
    {
        public int Timeout { get; set; }

        public override string ToxicType
        {
            get { return "timeout"; }
        }
    }
}