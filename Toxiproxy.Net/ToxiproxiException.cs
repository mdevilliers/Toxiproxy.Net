using System;
using System.Runtime.Serialization;

namespace Toxiproxy.Net
{
    [Serializable]
    public class ToxiProxiException : Exception
    {
        public ToxiProxiException()
        {
        }

        public ToxiProxiException(string message) : 
            base(message)
        {
        }

        public ToxiProxiException(string message, Exception inner) : 
            base(message, inner)
        {
        }

        protected ToxiProxiException(SerializationInfo info, StreamingContext context) : 
            base(info, context)
        {
        }

        public int Status { get; set; }
    }
}