using ProtoBuf;
using System.Xml;
using System.Xml.Serialization;

namespace dpp.cot
{
    [ProtoContract]
    public class Track
    {
        [ProtoMember(1, Name = @"speed")]
        [XmlAttribute(AttributeName = "speed")]
        public double Speed { get; set; }

        [ProtoMember(2, Name = @"course")]
        [XmlAttribute(AttributeName = "course")]
        public double Course { get; set; }

        [XmlAnyAttribute]
        public XmlAttribute[] AdditionalAttributes { get; set; } = System.Array.Empty<XmlAttribute>();
    }
}
