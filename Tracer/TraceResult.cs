using System.Collections.Generic;
using System.Xml.Serialization;

namespace Tracer
{

    [XmlRoot("root")]
    public class TraceResult
    {
        [XmlElement(ElementName = "thread")] public List<ThreadResult> Threads = new ();
    }
    public abstract class BaseResult
    {
        [XmlAttribute("time")]
        public long Time{ get; set; }
        [XmlElement(ElementName = "method")]
        public List<MethodResult> Methods{ get; set; }
    }
    public class ThreadResult : BaseResult
    {
        [XmlAttribute("id")] public int Id { get; set; }
    }
    public class MethodResult : BaseResult
    {
        [XmlAttribute("name")] public string Name { get; set; }

        [XmlAttribute("class")]
        public string ClassName{ get; set; }
    }
}