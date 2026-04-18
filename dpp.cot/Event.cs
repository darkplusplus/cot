using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace dpp.cot
{
	[XmlRoot(ElementName = "event")]
	public partial class Event
	{
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }

		[DefaultValue("")]
		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; } = "";

		[DefaultValue("")]
		[XmlAttribute(AttributeName = "access")]
		public string Access { get; set; } = "";

		[DefaultValue("")]
		[XmlAttribute(AttributeName = "qos")]
		public string Qos { get; set; } = "";

		[DefaultValue("")]
		[XmlAttribute(AttributeName = "opex")]
		public string Opex { get; set; } = "";

		[DefaultValue("")]
		[XmlAttribute(AttributeName = "caveat")]
		public string Caveat { get; set; } = "";

		[DefaultValue("")]
		[XmlAttribute(AttributeName = "releasableTo")]
		public string ReleaseableTo { get; set; } = "";

		[DefaultValue("")]
		[XmlAttribute(AttributeName = "uid")]
		public string Uid { get; set; } = "";

		private DateTime time;
		[XmlAttribute(AttributeName = "time")]
		public DateTime Time 
		{
			get { return time; }
			set { time = value; }
		}

		private DateTime start;
		[XmlAttribute(AttributeName = "start")]
		public DateTime Start
		{
			get { return start; }
			set { start = value; }
		}

		private DateTime stale;
		[XmlAttribute(AttributeName = "stale")]
		public DateTime Stale
		{
			get { return stale; }
			set { stale = value; }
		}

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
			var now = DateTime.UtcNow;
			Time = now;
			Start = now;
			Stale = now.AddMinutes(5);
		}

		public static Event Pong(Event ping = null)
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
			var settings = new XmlWriterSettings()
			{
				Indent = false,
				OmitXmlDeclaration = true,
				ConformanceLevel = ConformanceLevel.Document,
			};

            using var sw = new Utf8StringWriter();
            using var writer = XmlWriter.Create(sw, settings);

			writer.WriteStartElement("event");
			WriteOptionalAttribute(writer, "version", Version);
			WriteOptionalAttribute(writer, "uid", Uid);
			WriteOptionalAttribute(writer, "type", Type);
			WriteOptionalAttribute(writer, "access", Access);
			WriteOptionalAttribute(writer, "qos", Qos);
			WriteOptionalAttribute(writer, "opex", Opex);
			WriteOptionalAttribute(writer, "caveat", Caveat);
			WriteOptionalAttribute(writer, "releasableTo", ReleaseableTo);
			writer.WriteAttributeString("time", XmlConvert.ToString(ToUtc(Time), XmlDateTimeSerializationMode.Utc));
			writer.WriteAttributeString("start", XmlConvert.ToString(ToUtc(Start), XmlDateTimeSerializationMode.Utc));
			writer.WriteAttributeString("stale", XmlConvert.ToString(ToUtc(Stale), XmlDateTimeSerializationMode.Utc));
			WriteOptionalAttribute(writer, "how", How);

			WritePoint(writer, Point ?? new Point());
			WriteDetail(writer, Detail);

			writer.WriteEndElement();
			writer.Flush();

			return sw.ToString();
        }

		private static void WriteOptionalAttribute(XmlWriter writer, string name, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				writer.WriteAttributeString(name, value);
			}
		}

		private static void WritePoint(XmlWriter writer, Point point)
		{
			writer.WriteStartElement("point");
			writer.WriteAttributeString("lat", XmlConvert.ToString(point.Lat));
			writer.WriteAttributeString("lon", XmlConvert.ToString(point.Lon));
			writer.WriteAttributeString("hae", XmlConvert.ToString(point.Hae));
			writer.WriteAttributeString("ce", XmlConvert.ToString(point.Ce));
			writer.WriteAttributeString("le", XmlConvert.ToString(point.Le));
			writer.WriteEndElement();
		}

		private static void WriteDetail(XmlWriter writer, Detail detail)
		{
			if (detail == null)
			{
				return;
			}

			writer.WriteStartElement("detail");

			if (detail.Takv != null)
			{
				writer.WriteStartElement("takv");
				WriteOptionalAttribute(writer, "device", detail.Takv.Device);
				WriteOptionalAttribute(writer, "platform", detail.Takv.Platform);
				WriteOptionalAttribute(writer, "os", detail.Takv.Os);
				WriteOptionalAttribute(writer, "version", detail.Takv.Version);
				WriteAdditionalAttributes(writer, detail.Takv.AdditionalAttributes);
				writer.WriteEndElement();
			}

			if (detail.Contact != null)
			{
				writer.WriteStartElement("contact");
				WriteOptionalAttribute(writer, "endpoint", detail.Contact.Endpoint);
				WriteOptionalAttribute(writer, "callsign", detail.Contact.Callsign);
				WriteAdditionalAttributes(writer, detail.Contact.AdditionalAttributes);
				writer.WriteEndElement();
			}

			if (detail.PrecisionLocation != null)
			{
				writer.WriteStartElement("precisionlocation");
				WriteOptionalAttribute(writer, "geopointsrc", detail.PrecisionLocation.Geopointsrc);
				WriteOptionalAttribute(writer, "altsrc", detail.PrecisionLocation.Altsrc);
				WriteAdditionalAttributes(writer, detail.PrecisionLocation.AdditionalAttributes);
				writer.WriteEndElement();
			}

			if (detail.Group != null)
			{
				writer.WriteStartElement("__group");
				WriteOptionalAttribute(writer, "name", detail.Group.Name);
				WriteOptionalAttribute(writer, "role", detail.Group.Role);
				WriteAdditionalAttributes(writer, detail.Group.AdditionalAttributes);
				writer.WriteEndElement();
			}

			if (detail.Status != null)
			{
				writer.WriteStartElement("status");
				writer.WriteAttributeString("battery", XmlConvert.ToString(detail.Status.Battery));
				WriteAdditionalAttributes(writer, detail.Status.AdditionalAttributes);
				writer.WriteEndElement();
			}

			if (detail.Track != null)
			{
				writer.WriteStartElement("track");
				writer.WriteAttributeString("speed", XmlConvert.ToString(detail.Track.Speed));
				writer.WriteAttributeString("course", XmlConvert.ToString(detail.Track.Course));
				WriteAdditionalAttributes(writer, detail.Track.AdditionalAttributes);
				writer.WriteEndElement();
			}

			if (detail.AdditionalElements != null)
			{
				foreach (var element in detail.AdditionalElements)
				{
					element.WriteTo(writer);
				}
			}

			if (!string.IsNullOrWhiteSpace(detail.xmlDetail) &&
				(detail.AdditionalElements == null || detail.AdditionalElements.Length == 0))
			{
				writer.WriteRaw(detail.xmlDetail);
			}

			writer.WriteFullEndElement();
		}

		private static void WriteAdditionalAttributes(XmlWriter writer, XmlAttribute[] attributes)
		{
			if (attributes == null)
			{
				return;
			}

			foreach (var attribute in attributes)
			{
				if (attribute == null)
				{
					continue;
				}

				writer.WriteAttributeString(attribute.Prefix, attribute.LocalName, attribute.NamespaceURI, attribute.Value);
			}
		}

		private static DateTime ToUtc(DateTime value)
		{
			return value.Kind switch
			{
				DateTimeKind.Utc => value,
				DateTimeKind.Local => value.ToUniversalTime(),
				_ => DateTime.SpecifyKind(value, DateTimeKind.Utc),
			};
		}

		private sealed class Utf8StringWriter : StringWriter
		{
			public override Encoding Encoding => Encoding.UTF8;
		}
	}
}
