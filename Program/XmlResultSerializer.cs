using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Tracer;

namespace Program
{
    namespace Serializer
    {
        class XmlResultSerializer : ISerializer
        {
            private XmlSerializerNamespaces emptyNamespaces = new (new[] { XmlQualifiedName.Empty });
            public string Serialize(object obj)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
                using (StringWriter stringWriter = new StringWriter())
                {
                    using (XmlWriter.Create(stringWriter))
                    {
                        xmlSerializer.Serialize(stringWriter, obj, emptyNamespaces);
                        return stringWriter.ToString();
                    }
                }
            }
        }
    }
}