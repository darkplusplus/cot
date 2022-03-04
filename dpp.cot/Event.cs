using ProtoBuf;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace dpp.cot
{
	[ProtoContract]
	[XmlRoot(ElementName = "event")]
	public partial class Event : IExtensible
	{
		private IExtension __pbn__extensionData;
		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			=> Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }

		[ProtoMember(1, Name = @"type")]
		[DefaultValue("")]
		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; } = "";

		[ProtoMember(2, Name = @"access")]
		[DefaultValue("")]
		[XmlAttribute(AttributeName = "access")]
		public string Access { get; set; } = "";

		[ProtoMember(3, Name = @"qos")]
		[DefaultValue("")]
		[XmlAttribute(AttributeName = "qos")]
		public string Qos { get; set; } = "";

		[ProtoMember(4, Name = @"opex")]
		[DefaultValue("")]
		[XmlAttribute(AttributeName = "opex")]
		public string Opex { get; set; } = "";

		[ProtoMember(5, Name = @"uid")]
		[DefaultValue("")]
		[XmlAttribute(AttributeName = "uid")]
		public string Uid { get; set; } = "";

		[ProtoMember(6)]
		private ulong _time;
		private DateTime time;
		[XmlAttribute(AttributeName = "time")]
		public DateTime Time 
		{
			get { return time; }
			set { time = value; _time = (ulong)time.Ticks; }
		}

		[ProtoMember(7)]
		private ulong _start;
		private DateTime start;
		[XmlAttribute(AttributeName = "start")]
		public DateTime Start
		{
			get { return start; }
			set { start = value; _start = (ulong)start.Ticks; }
		}

		[ProtoMember(8)]
		private ulong _stale;
		private DateTime stale;
		[XmlAttribute(AttributeName = "stale")]
		public DateTime Stale
		{
			get { return stale; }
			set { stale = value; _stale = (ulong)stale.Ticks; }
		}

		[ProtoMember(9, Name = @"how")]
		[DefaultValue("")]
		[XmlAttribute(AttributeName = "how")]
		public string How { get; set; } = "";

		[XmlElement(ElementName = "point")]
		public Point Point { get; set; }

		[XmlElement(ElementName = "detail", IsNullable = true)]
		public Detail Detail { get; set; }

		public Event()
        {
			this.Point = new Point();
        }

		public static Event Pong()
		{
            var e = new Event{Type = "t-x-c-t-r"};

            return e;
		}

		public static Event Parse(string payload)
		{
			var serializer = new XmlSerializer(typeof(Event));
            using var reader = new StringReader(payload);
            return (Event)(serializer.Deserialize(reader));
        }

		public static Event Parse(byte[] payload, int offset, int length)
        {
			// messages are in the form <magic><version><magic><data>

			const byte magic = 0xbf;
			const byte v1 = 0x00; // UTF-8 encoded xml
			const byte v2 = 0x01; // Protobuf

			if (payload.Length >= 3 && payload[0] == magic && payload[0] == payload[2])
            {
				if (payload[1] == v1)
                {
                    return Parse(Encoding.UTF8.GetString((byte[])payload.Skip(3)));
                }
				if (payload[1] == v2)
                {
					throw new NotImplementedException("No protobuf support");
                }
            }

			// no magic preamble so assume it's just a uft8 string
			return Parse(Encoding.UTF8.GetString(payload));
		}

		public String ToXmlString()
		{
            // empty namespaces to force serializer to omit them
            var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

			var settings = new XmlWriterSettings()
			{
				Indent = false,
				OmitXmlDeclaration = true,
				ConformanceLevel = ConformanceLevel.Auto,
			};

            using MemoryStream ms = new();
            using (XmlWriter writer = XmlWriter.Create(ms, settings))
            {
                var serializer = new XmlSerializer(typeof(Event), "");
                serializer.Serialize(writer, this, ns);
            }

            var encoding = new UTF8Encoding();
            var result = encoding.GetString(ms.ToArray());

            // fix BOM and self closing tags quirk 
            result = result.Replace("\ufeff", "");
            result = result.Replace("<detail />", "<detail></detail>");

            return result;
        }
	}
}


