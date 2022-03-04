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
    public partial class Contact : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"endpoint")]
        [DefaultValue("")]
        [XmlAttribute(AttributeName = "endpoint")]
        public string Endpoint { get; set; } = "";

        [ProtoMember(2, Name = @"callsign")]
        [DefaultValue("")]
        [XmlAttribute(AttributeName = "callsign")]
        public string Callsign { get; set; } = "";

    }
}
