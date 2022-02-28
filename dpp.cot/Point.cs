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

		[XmlAttribute(AttributeName = "lat")]
		public double Lat { get; set; } = 0;

		[XmlAttribute(AttributeName = "lon")]
		public double Lon { get; set; } = 0;

		[XmlAttribute(AttributeName = "ce")]
		public double Ce { get; set; } = 9999999.0;

		[XmlAttribute(AttributeName = "hae")]
		public double Hae { get; set; } = 9999999.0;

		[XmlAttribute(AttributeName = "le")]
		public double Le { get; set; } = 9999999.0;
	}
}
