using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace dpp.cot
{
    [ProtoContract()]
    public partial class Group : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"name")]
        [DefaultValue("")]
        [XmlAttribute(AttributeName = @"name")]
        public string Name { get; set; } = "";

        [ProtoMember(2, Name = @"role")]
        [DefaultValue("")]
        [XmlAttribute(AttributeName = @"role")]
        public string Role { get; set; } = "";

    }
}
