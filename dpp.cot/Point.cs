using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace dpp.cot
{
	[XmlRoot(ElementName = "point")]
	public class Point
	{
		[ProtoMember(10, Name = @"lat")]
		[XmlAttribute(AttributeName = "lat")]
		public double Lat { get; set; } = 0;

		[ProtoMember(11, Name = @"lon")]
		[XmlAttribute(AttributeName = "lon")]
		public double Lon { get; set; } = 0;

		[ProtoMember(12, Name = @"ce")]
		[XmlAttribute(AttributeName = "ce")]
		public double Ce { get; set; } = 9999999.0;

		[ProtoMember(13, Name = @"hae")]
		[XmlAttribute(AttributeName = "hae")]
		public double Hae { get; set; } = 9999999.0;

		[ProtoMember(14, Name = @"le")]
		[XmlAttribute(AttributeName = "le")]
		public double Le { get; set; } = 9999999.0;
	}
}
