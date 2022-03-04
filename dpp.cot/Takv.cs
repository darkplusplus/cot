using ProtoBuf;
using System.ComponentModel;
using System.Xml.Serialization;

namespace dpp.cot
{
    [ProtoContract()]
    public partial class Takv : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"device")]
        [DefaultValue("")]
        [XmlAttribute(AttributeName = "device")]
        public string Device { get; set; } = "";

        [ProtoMember(2, Name = @"platform")]
        [DefaultValue("")]
        [XmlAttribute(AttributeName = "platform")]
        public string Platform { get; set; } = "";

        [ProtoMember(3, Name = @"os")]
        [DefaultValue("")]
        [XmlAttribute(AttributeName = "os")]
        public string Os { get; set; } = "";

        [ProtoMember(4, Name = @"version")]
        [DefaultValue("")]
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; } = "";
    }
}