using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpp.cot
{
    [ProtoContract()]
    public partial class Message : IExtensible
    {
        // versioning bytes for header
        // magic|version|magic|data
        private const byte magic = 0xef;
        private const byte v0_xml = 0x00;
        private const byte v1_protobuf = 0x01;

        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1)]
        public TakControl Control { get; set; }

        [ProtoMember(2)]
        public Event Event { get; set; }

        public static Message Parse(byte[] data, int offset, int length)
        {
            string msg;
            Event e;

            if (data[0] == magic && data[2] == magic)
            {
                switch (data[1])
                {
                    case v0_xml:
                        msg = Encoding.UTF8.GetString(data, (int)offset + 3, (int)length);
                        e = Event.Parse(msg);

                        return new Message
                        {
                            Event = e,
                            Control = new TakControl()
                            {
                                minProtoVersion = v0_xml,
                                maxProtoVersion = v0_xml,
                                contactUid = e.Uid,
                            }
                        };

                    case v1_protobuf:
                        using (var ms = new MemoryStream(data, offset + 3, length))
                        {
                            return ProtoBuf.Serializer.Deserialize<Message>(ms);
                        }

                    default:
                        throw new NotImplementedException($"Unknown protocol version. Version={data[1].ToString("X")}");
                }
            }

            // No magic bytes detected to specify version, assume it's just utf8 xml
            msg = Encoding.UTF8.GetString(data, (int)offset, (int)length);
            e = Event.Parse(msg);

            return new Message
            {
                Event = e,
                Control = new TakControl()
                {
                    minProtoVersion = 0,
                    maxProtoVersion = 0,
                    contactUid = e.Uid,
                }
            };
        }

        public string ToXmlString()
        {
            return this.Event.ToXmlString();
        }

        public byte[] ToXmlBytes()
        {
            var header = new byte[]{
                magic,
                v0_xml,
                magic,
            };
            var data = Encoding.UTF8.GetBytes(this.Event.ToXmlString());

            return header.Concat(data).ToArray();
        }
    }
}
