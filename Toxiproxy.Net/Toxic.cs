
using System;
using RestSharp.Deserializers;

namespace Toxiproxy.Net
{
    internal enum ToxicDirection
    {
        UpStream,
        DownStream
    }

    public abstract class Toxic
    {
        public bool Enabled { get; set; }

        public void Update()
        {
            if (Direction == ToxicDirection.DownStream)
            {
                Client.UpdateDownStreamToxic(ParentProxy, this);
            }
            else
            {
                Client.UpdateUpStreamToxic(ParentProxy, this);
            }
        }

        internal abstract string ToxicType { get; }
        internal Client Client { get; set; }
        internal string ParentProxy { get; set; }
        internal ToxicDirection Direction { get; set; }
    }

    public class LatencyToxic : Toxic
    {
        public int Latency { get; set; }
        public int Jitter { get; set; }

        internal override string ToxicType
        {
            get { return "latency"; }
        }
    }

    public class SlowCloseToxic : Toxic
    {
        public int Delay { get; set; }

        internal override string ToxicType
        {
            get { return "slow_close"; }
        }
    }

    public class TimeoutToxic : Toxic
    {
        public int Timeout { get; set; }

        internal override string ToxicType
        {
            get { return "timeout"; }
        }
    }

    public class BandwidthToxic : Toxic
    {
        public Int64 Rate { get; set; }

        internal override string ToxicType
        {
            get { return "bandwidth"; }
        }
    }

    public class SlicerToxic : Toxic
    {
        [DeserializeAs(Name = "average_size")]
        public int Average_Size { get; set; }

        [DeserializeAs(Name = "size_variation")]
        public int Size_Variation { get; set; }

        public int Delay { get; set; }

        internal override string ToxicType
        {
            get { return "slicer"; }
        }
    }
}