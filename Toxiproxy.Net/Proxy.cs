
namespace Toxiproxy.Net
{
    public class Proxy
    {
        public string Name { get; set; }

        public string Listen { get; set; }

        public string Upstream { get; set; }

        public bool Enabled { get; set; }

        public ToxicCollection UpStreams()
        {
            return Client.FindUpStreamToxicsForProxy(this);
        }

        public ToxicCollection DownStreams()
        {
            return Client.FindDownStreamToxicsForProxy(this);
        }

        public void Delete()
        {
            Client.Delete(this);
        }

        public Proxy Update()
        {
            return Client.Update(this);         
        }

        internal Client Client { get; set; }
        
    }
}