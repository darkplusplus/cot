using ProtoBuf;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
			Point = new Point();
			Time = DateTime.Now;
			Start = DateTime.Now;
			Stale = DateTime.Now.AddMinutes(5);
		}

		public static Event Pong(Event? ping)
		{
            var e = ping ?? new Event();
			e.Uid ??= Guid.NewGuid().ToString();
			e.Type = "t-x-c-t-r";

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
			return Parse(Encoding.UTF8.GetString(payload, offset, length));
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

            // fix BOM, self closing tags quirk, and namespace from default values
            result = result.Replace("\ufeff", "");
            result = result.Replace("<detail />", "<detail></detail>");
			result = Regex.Replace(result, @"(xmlns:)?p3(:nil)?.+//", "");

            return result;
        }
	}
}


