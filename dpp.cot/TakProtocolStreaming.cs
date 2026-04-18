using ProtoBuf;
using System;
using System.IO;
using System.Text;

namespace dpp.cot
{
    public static class TakProtocolStreaming
    {
        private const byte StreamingMagic = 0xbf;
        private const byte XmlVersion = 0x00;
        private const byte ProtobufVersion = 0x01;

        public static byte[] ToStreamBytes(Message message, byte protocolVersion)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var payload = protocolVersion switch
            {
                XmlVersion => Encoding.UTF8.GetBytes(message.ToXmlString()),
                ProtobufVersion => SerializeProtobufPayload(message),
                _ => throw new NotImplementedException($"Unknown protocol version. Version={protocolVersion:X}"),
            };

            using var ms = new MemoryStream();
            ms.WriteByte(StreamingMagic);
            WriteUnsignedVarint(ms, (ulong)payload.Length);
            ms.Write(payload, 0, payload.Length);
            return ms.ToArray();
        }

        public static Message ParseStreamMessage(byte[] data, int offset, int length, byte protocolVersion)
        {
            if (!TryParseStreamMessage(data, offset, length, protocolVersion, out var message, out var bytesConsumed))
            {
                throw new ArgumentException("Incomplete streaming message.", nameof(length));
            }

            if (bytesConsumed != length)
            {
                throw new ArgumentException("The supplied segment contains extra bytes beyond a single streaming message.", nameof(length));
            }

            return message;
        }

        public static bool TryParseStreamMessage(byte[] data, int offset, int length, byte protocolVersion, out Message message, out int bytesConsumed)
        {
            message = null;
            bytesConsumed = 0;

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
                return false;
            }

            if (data[offset] != StreamingMagic)
            {
                throw new NotImplementedException($"Unknown stream magic byte. Value={data[offset]:X}");
            }

            if (!TryReadUnsignedVarint(data, offset + 1, length - 1, out var payloadLength, out var headerBytes))
            {
                return false;
            }

            if (payloadLength > int.MaxValue)
            {
                throw new NotSupportedException($"Streaming payload too large. Length={payloadLength}");
            }

            var totalLength = 1 + headerBytes + (int)payloadLength;
            if (totalLength > length)
            {
                return false;
            }

            var payloadOffset = offset + 1 + headerBytes;
            message = protocolVersion switch
            {
                XmlVersion => ParseXmlPayload(data, payloadOffset, (int)payloadLength),
                ProtobufVersion => ParseProtobufPayload(data, payloadOffset, (int)payloadLength),
                _ => throw new NotImplementedException($"Unknown protocol version. Version={protocolVersion:X}"),
            };

            bytesConsumed = totalLength;
            return true;
        }

        private static Message ParseXmlPayload(byte[] data, int offset, int length)
        {
            var evt = Event.Parse(data, offset, length);
            return new Message
            {
                Event = evt,
                Control = new TakControl
                {
                    minProtoVersion = XmlVersion,
                    maxProtoVersion = XmlVersion,
                }
            };
        }

        private static Message ParseProtobufPayload(byte[] data, int offset, int length)
        {
            using var ms = new MemoryStream(data, offset, length, writable: false);
            var proto = Serializer.Deserialize<ProtoTakMessage>(ms);
            return CotProtoMapper.FromProto(proto);
        }

        private static byte[] SerializeProtobufPayload(Message message)
        {
            using var ms = new MemoryStream();
            Serializer.Serialize(ms, CotProtoMapper.ToProto(message));
            return ms.ToArray();
        }

        private static void WriteUnsignedVarint(Stream stream, ulong value)
        {
            while (value >= 0x80)
            {
                stream.WriteByte((byte)(value | 0x80));
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
                var chunk = (ulong)(current & 0x7f);
                value |= chunk << (7 * bytesRead);
                bytesRead++;

                if ((current & 0x80) == 0)
                {
                    return true;
                }
            }

            if (bytesRead == maxVarintBytes && length >= maxVarintBytes)
            {
                throw new FormatException("Invalid TAK streaming varint header.");
            }

            value = 0;
            bytesRead = 0;
            return false;
        }
    }
}
