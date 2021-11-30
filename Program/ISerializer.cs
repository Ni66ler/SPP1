using Tracer;

namespace Program
{
    namespace Serializer
    {
        interface ISerializer
        {
            public string Serialize(object obj);
        }
    }
}