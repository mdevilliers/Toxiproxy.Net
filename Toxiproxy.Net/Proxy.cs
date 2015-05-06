
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

        public void Update()
        {
            var response = Client.Update(this);

            this.Enabled = response.Enabled;
            this.Listen = response.Listen;
            this.Upstream = response.Upstream;
        }

        internal Client Client { get; set; }
        
    }
}