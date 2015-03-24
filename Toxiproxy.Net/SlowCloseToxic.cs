namespace Toxiproxy.Net
{
    public class SlowCloseToxic : Toxic
    {
        public int Delay { get; set; }

        public override string ToxicType
        {
            get { return "slow_close"; }
        }
    }
}