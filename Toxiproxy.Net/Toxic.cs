
using System;
using RestSharp.Deserializers;
using RestSharp;

namespace Toxiproxy.Net
{
    public enum ToxicDirection
    {
        UpStream,
        DownStream
    }

    public abstract class Toxic
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public ToxicDirection Stream { get; set; }
        internal abstract string ToxicType { get; }

        internal Client Client { get; set; }
        internal string ParentProxy { get; set; }

        public string Serialize() {
            return SimpleJson.SerializeObject(this);
        }
    }

    public class LatencyToxic : Toxic
    {
        public int Latency { get; set; }
        public int Jitter { get; set; }

        internal override string ToxicType
        {
            get { return "latency"; }
        }

        public override bool Equals(object obj)
        {
            var t = obj as LatencyToxic;
            return t != null ? this.Equals(t) : base.Equals(obj);
        }

        protected bool Equals(LatencyToxic other)
        {
            return Latency == other.Latency && Jitter == other.Jitter && Enabled == other.Enabled;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Latency * 397) ^ Jitter;
            }
        }
    }

    public class SlowCloseToxic : Toxic
    {
        public int Delay { get; set; }

        internal override string ToxicType
        {
            get { return "slow_close"; }
        }

        public override bool Equals(object obj)
        {
            var t = obj as SlowCloseToxic;
            return t != null ? this.Equals(t) : base.Equals(obj);
        }

        protected bool Equals(SlowCloseToxic other)
        {
            return Delay == other.Delay && Enabled == other.Enabled;
        }

        public override int GetHashCode()
        {
            return Delay;
        }

    }

    public class TimeoutToxic : Toxic
    {
        public int Timeout { get; set; }

        internal override string ToxicType
        {
            get { return "timeout"; }
        }

        public override bool Equals(object obj)
        {
            var t = obj as TimeoutToxic;
            return t != null ? this.Equals(t) : base.Equals(obj);
        }

        protected bool Equals(TimeoutToxic other)
        {
            return Timeout == other.Timeout && Enabled == other.Enabled;
        }

        public override int GetHashCode()
        {
            return Timeout;
        }
    }

    public class BandwidthToxic : Toxic
    {
        public long Rate { get; set; }

        internal override string ToxicType
        {
            get { return "bandwidth"; }
        }

        public override bool Equals(object obj)
        {
            var t = obj as BandwidthToxic;
            return t != null ? Equals(t) : base.Equals(obj);
        }

        protected bool Equals(BandwidthToxic other)
        {
            return Rate == other.Rate && Enabled == other.Enabled;
        }

        public override int GetHashCode()
        {
            return Rate.GetHashCode();
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

        public override bool Equals(object obj)
        {
            var t = obj as SlicerToxic;
            return t != null ? this.Equals(t) : base.Equals(obj);
        }

        protected bool Equals(SlicerToxic other)
        {
            return Average_Size == other.Average_Size && Size_Variation == other.Size_Variation && Delay == other.Delay && Enabled == other.Enabled;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Average_Size;
                hashCode = (hashCode * 397) ^ Size_Variation;
                hashCode = (hashCode * 397) ^ Delay;
                return hashCode;
            }
        }
    }
}