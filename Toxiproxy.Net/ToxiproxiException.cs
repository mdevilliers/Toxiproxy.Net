using System;
using System.Runtime.Serialization;

namespace Toxiproxy.Net
{
    [Serializable]
    public class ToxiproxiException : Exception
    {
        public ToxiproxiException()
        {
        }

        public ToxiproxiException(string message) : base(message)
        {
        }

        public ToxiproxiException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ToxiproxiException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public int Status { get; set; }
    }
}