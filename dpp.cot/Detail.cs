using System.Xml;
using System.Xml.Serialization;

namespace dpp.cot
{
    public class Detail
    {
        // Residual detail XML used by the protobuf mapping layer.
        public string xmlDetail { get; set; } = "";

        [XmlElement(ElementName = "contact", IsNullable = true)]
        public Contact Contact { get; set; }

        [XmlElement(ElementName = "__group", IsNullable = true)]
        public Group Group { get; set; }

        [XmlElement(ElementName = @"precisionlocation", IsNullable = true)]
        public PrecisionLocation PrecisionLocation { get; set; }

        [XmlElement(ElementName = "status", IsNullable = true)]
        public Status Status { get; set; }

        [XmlElement(ElementName = "takv", IsNullable = true)]
        public Takv Takv { get; set; }

        [XmlElement(ElementName = "track", IsNullable = true)]
        public Track Track { get; set; }

        [XmlAnyElement]
        public XmlElement[] AdditionalElements { get; set; } = System.Array.Empty<XmlElement>();
    }
}
