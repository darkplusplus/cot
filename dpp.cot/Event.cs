using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace dpp.cot
{
	[XmlRoot(ElementName = "event")]
	public class Event
	{
		
		[XmlElement(ElementName = "detail", IsNullable = true)]
		public object Detail { get; set; }

		[XmlElement(ElementName = "point")]
		public Point Point { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }

		[XmlAttribute(AttributeName = "uid")]
		public string Uid { get; set; } = Guid.NewGuid().ToString();

		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }

		[XmlAttribute(AttributeName = "time")]
		public DateTime Time { get; set; } = DateTime.Now;

		[XmlAttribute(AttributeName = "start")]
		public DateTime Start { get; set; } = DateTime.Now;

		[XmlAttribute(AttributeName = "stale")]
		public DateTime Stale { get; set; } = DateTime.Now.AddMinutes(5);

		public Event()
        {
			this.Point = new Point();
        }

		public static Event Pong()
		{
			var e = new Event();
			e.Type = "t-x-c-t-r";

			return e;
		}

		public static Event Parse(string payload)
		{
			var serializer = new XmlSerializer(typeof(Event));
			using (var reader = new StringReader(payload))
			{
				return (Event)(serializer.Deserialize(reader));
			}
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
			
			using (var ms = new MemoryStream())
            {
				using (var writer = XmlWriter.Create(ms, settings))
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
}


