using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toxiproxy.Net.V2
{
    // TODO - Duplicated with the other Proxy class
    public class Proxy
    {
        // TODO - Duplicated with the other Proxy class
        public string Name { get; set; }

        // TODO - Duplicated with the other Proxy class
        public string Listen { get; set; }

        // TODO - Duplicated with the other Proxy class
        public bool Enabled { get; set; }

        // TODO - Duplicated with the other Proxy class
        public void Delete()
        {
            Client.Delete(this);
        }

        // TODO - Duplicated with the other Proxy class
        public Proxy Update()
        {
            return Client.Update(this);
        }

        // TODO - Duplicated with the other Proxy class
        internal Client Client { get; set; }
    }
}
