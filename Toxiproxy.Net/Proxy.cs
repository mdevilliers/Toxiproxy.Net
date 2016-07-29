
using System;

namespace Toxiproxy.Net
{
    public class Proxy
    {
        public string Name { get; set; }
        public string Listen { get; set; }
        public string Upstream { get; set; }

        public bool Enabled { get; set; }
        
        public void Delete()
        {
            Client.Delete(this);
        }

        public Proxy Update()
        {
            return Client.Update(this);         
        }

        public T Add<T>(T toxic) where T : Toxic
        {
            throw new NotImplementedException();
        }

        internal Client Client { get; set; }
    }
}