using ProtoBuf;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace dpp.cot
{
    public partial class Message
    {
        // versioning bytes for header
        // magic|version|magic|data
        private const byte magic = 0xef;
        private const byte v0_xml = 0x00;
        private const byte v1_protobuf = 0x01;

        public TakControl Control { get; set; }

        public Event Event { get; set; }

        public DateTime? SubmissionTime { get; set; }

        public DateTime? CreationTime { get; set; }

        public static Message Parse(byte[] data, int offset, int length)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || length < 0 || offset + length > data.Length)
            {
                throw new ArgumentOutOfRangeException($"Invalid segment. offset={offset}, length={length}, data.Length={data.Length}");
            }

            if (length == 0)
            {
                throw new ArgumentException("Message payload cannot be empty.", nameof(length));
            }

            string msg;
            Event e;

            if (length >= 3 && data[offset] == magic)
            {
                if (!TryReadUnsignedVarint(data, offset + 1, length - 1, out var version, out var versionBytes))
                {
                    throw new ArgumentException("Incomplete TAK mesh protocol header.", nameof(length));
                }

                var trailingMagicOffset = offset + 1 + versionBytes;
                if (trailingMagicOffset >= offset + length || data[trailingMagicOffset] != magic)
                {
                    throw new NotImplementedException("Failed to find TAK mesh trailing magic byte.");
                }

                var payloadOffset = trailingMagicOffset + 1;
                var payloadLength = (offset + length) - payloadOffset;

                switch (version)
                {
                    case v0_xml:
                        msg = Encoding.UTF8.GetString(data, payloadOffset, payloadLength);
                        e = Event.Parse(msg);

                        return new Message
                        {
                            Event = e,
                            Control = new TakControl()
                            {
                                minProtoVersion = v0_xml,
                                maxProtoVersion = v0_xml,
                            }
                        };

                    case v1_protobuf:
                        using (var ms = new MemoryStream(data, payloadOffset, payloadLength, writable: false))
                        {
                            var proto = Serializer.Deserialize<ProtoTakMessage>(ms);
                            return CotProtoMapper.FromProto(proto);
                        }

                    default:
                        throw new NotImplementedException($"Unknown protocol version. Version={version:X}");
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
                }
            };
        }

        public string ToXmlString()
        {
            return this.Event.ToXmlString();
        }

        public byte[] ToXmlBytes()
        {
            var header = BuildMeshHeader(v0_xml);
            var data = Encoding.UTF8.GetBytes(this.Event.ToXmlString());

            return header.Concat(data).ToArray();
        }

        public byte[] ToProtobufBytes()
        {
            var header = BuildMeshHeader(v1_protobuf);

            using var ms = new MemoryStream();
            Serializer.Serialize(ms, CotProtoMapper.ToProto(this));

            return header.Concat(ms.ToArray()).ToArray();
        }

        public byte[] ToStreamingBytes(byte protocolVersion)
        {
            return TakProtocolStreaming.ToStreamBytes(this, protocolVersion);
        }

        public static Message ParseStreaming(byte[] data, int offset, int length, byte protocolVersion)
        {
            return TakProtocolStreaming.ParseStreamMessage(data, offset, length, protocolVersion);
        }

        public static bool TryParseStreaming(byte[] data, int offset, int length, byte protocolVersion, out Message message, out int bytesConsumed)
        {
            return TakProtocolStreaming.TryParseStreamMessage(data, offset, length, protocolVersion, out message, out bytesConsumed);
        }

        private static byte[] BuildMeshHeader(byte version)
        {
            using var ms = new MemoryStream();
            ms.WriteByte(magic);
            WriteUnsignedVarint(ms, version);
            ms.WriteByte(magic);
            return ms.ToArray();
        }

        private static void WriteUnsignedVarint(Stream stream, ulong value)
        {
            while (value >= 0x80)
            {
                stream.WriteByte((byte)((value & 0x7f) | 0x80));
                value >>= 7;
            }

            stream.WriteByte((byte)value);
        }

        private static bool TryReadUnsignedVarint(byte[] data, int offset, int length, out ulong value, out int bytesRead)
        {
            value = 0;
            bytesRead = 0;
            const int maxVarintBytes = 10;

            while (bytesRead < length && bytesRead < maxVarintBytes)
            {
                var current = data[offset + bytesRead];
                value |= (ulong)(current & 0x7f) << (7 * bytesRead);
                bytesRead++;

                if ((current & 0x80) == 0)
                {
                    return true;
                }
            }

            if (bytesRead == maxVarintBytes && length >= maxVarintBytes)
            {
                throw new FormatException("Invalid TAK mesh varint header.");
            }

            value = 0;
            bytesRead = 0;
            return false;
        }
    }
}
