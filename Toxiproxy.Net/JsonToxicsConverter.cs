using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Toxiproxy.Net.Toxics;

namespace Toxiproxy.Net
{
    /// <summary>
    /// This is a custom JsonConverter, to be able to deserialize collection
    /// of entities that derive from ToxicBase
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.JsonConverter" />
    public class JsonToxicsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ToxicBase).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var typeName = jsonObject["type"].ToString();
            switch (typeName)
            {
                case ToxicTypenames.LatencyToxic:
                    return jsonObject.ToObject<LatencyToxic>();
                case ToxicTypenames.SlowCloseToxic:
                    return jsonObject.ToObject<SlowCloseToxic>();
                case ToxicTypenames.TimeoutToxic:
                    return jsonObject.ToObject<TimeoutToxic>();
                case ToxicTypenames.BandwidthToxic:
                    return jsonObject.ToObject<BandwidthToxic>();
                case ToxicTypenames.SlicerToxic:
                    return jsonObject.ToObject<SlicerToxic>();
                default:
                    throw new InvalidOperationException("Unknow type: " + typeName);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
