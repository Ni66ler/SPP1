using Newtonsoft.Json;
using Tracer;

namespace Program
{
    namespace Serializer
    {
        public class JsonResultSerializer : ISerializer
        {
            private JsonSerializerSettings JsonSettings = new ()
            {
                NullValueHandling = NullValueHandling.Include,
                Formatting = Formatting.Indented,
            };
            public string Serialize(object obj)
            {
                return JsonConvert.SerializeObject(obj, JsonSettings);
            }
        }
    }
}