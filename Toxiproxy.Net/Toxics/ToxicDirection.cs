using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Toxiproxy.Net.Toxics
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ToxicDirection
    {
        UpStream,
        DownStream
    }
}
