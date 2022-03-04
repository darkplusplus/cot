using ProtoBuf;
using System.ComponentModel;

namespace dpp.cot
{
    [ProtoContract()]
    public partial class TakControl : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1)]
        public uint minProtoVersion { get; set; }

        [ProtoMember(2)]
        public uint maxProtoVersion { get; set; }

        [ProtoMember(3)]
        [DefaultValue("")]
        public string contactUid { get; set; } = "";
    }
}