using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dpp.cot.Tests
{
    public class SerializationTests
    {
        [Theory]
        [InlineData(Helpers.SimplePayload, "J-01334")]
        [InlineData(Helpers.EudPayload, "ANDROID-ASDFASDFASDF")]
        public void BasicDeserializationUidTest(string corpus, string expected)
        {
            var evt = cot.Event.Parse(corpus);

            Assert.Equal(expected, evt.Uid);
        }

        [Theory]
        [InlineData(Helpers.SimplePayload, "a-h-A-M-F-U-M")]
        [InlineData(Helpers.EudPayload, "a-f-G-U-C")]
        public void BasicDeserializationTypeStringTest(string corpus, string expected)
        {
            var evt = cot.Event.Parse(corpus);

            Assert.Equal(expected, evt.Type);
        }

        [Fact]
        public void BasicDeserializationTimeTest()
        {
            string corpus = Helpers.SimplePayload;

            var evt = cot.Event.Parse(corpus);

            Assert.Equal(new System.DateTime(2005, 4, 5, 11, 43, 38, 70, System.DateTimeKind.Utc), evt.Time);
        }

        [Fact]
        public void XmlDoesntContainNamespaces()
        {
            var corpus = Helpers.SimplePayload;
            var evt = cot.Event.Parse(corpus);
            var xml = evt.ToXmlString();

            Assert.DoesNotContain("xmlns", xml);
        }

        [Theory]
        [InlineData(Helpers.SimplePayload, 30.0090027, -85.9578735)]
        [InlineData(Helpers.EudPayload, -0.00123456789012345, -0.00123456789012345)]
        public void BasicDeserializationLatLonTest(string corpus, double expected_lat, double expected_lon)
        {
            var evt = cot.Event.Parse(corpus);

            Assert.Equal(expected_lat, evt.Point.Lat);
            Assert.Equal(expected_lon, evt.Point.Lon);
        }

        [Theory(Skip="Figure out quotes and element ordering later")]
        [InlineData(Helpers.SimplePayload)] // TODO: xml tag ordering?
        [InlineData(Helpers.EudPayload)]  // TODO: do quote types actually matter?
        public void BasicXmlDeserializeXmlSerializeTest(string corpus)
        {
            var evt = cot.Event.Parse(corpus);
            var xml = evt.ToXmlString();

            Assert.Equal(corpus, xml);
        }
    }
}
