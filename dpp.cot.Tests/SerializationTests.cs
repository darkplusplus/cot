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
            Assert.DoesNotContain("w3.org", xml);
        }

        [Fact]
        public void SimplePayloadIntegrity()
        {
            var corpus = Helpers.SimplePayload;
            var evt = cot.Event.Parse(corpus);

            Assert.Equal("J-01334", evt.Uid);
            Assert.Equal("a-h-A-M-F-U-M", evt.Type);
            Assert.Equal(30.0090027, evt.Point.Lat);
            Assert.Equal(-85.9578735, evt.Point.Lon);
            Assert.Equal(45.3, evt.Point.Ce);
            Assert.Equal(-42.6, evt.Point.Hae);
        }

        [Fact]
        public void EudPayloadIntegrity()
        {
            var corpus = Helpers.EudPayload;
            var evt = Event.Parse(corpus);

            Assert.Equal("ANDROID-ASDFASDFASDF", evt.Uid);
            Assert.Equal("a-f-G-U-C", evt.Type);
            Assert.Equal("h-e", evt.How);
            Assert.Equal(-0.00123456789012345, evt.Point.Lat);
            Assert.Equal(-0.00123456789012345, evt.Point.Lon);
            Assert.Equal(9999999.0, evt.Point.Ce);
            Assert.Equal(9999999.0, evt.Point.Hae);
            Assert.Equal(9999999.0, evt.Point.Le);
            Assert.Equal("SOMSANG FOOBAR", evt.Detail.Takv.Device);
            Assert.Equal("ATAK-CIV", evt.Detail.Takv.Platform);
            Assert.Equal("25", evt.Detail.Takv.Os);
            Assert.Equal("1.23.4-56789.56789-CIV", evt.Detail.Takv.Version);
            Assert.Equal("192.168.1.2:4242:tcp", evt.Detail.Contact.Endpoint);
            Assert.Equal("SUPERMAN", evt.Detail.Contact.Callsign);
            Assert.Equal("USER", evt.Detail.PrecisionLocation.Geopointsrc);
            Assert.Equal("???", evt.Detail.PrecisionLocation.Altsrc);
            Assert.Equal("Blue", evt.Detail.Group.Name);
            Assert.Equal("Team Member", evt.Detail.Group.Role);
            Assert.Equal((uint)45, evt.Detail.Status.Battery);
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
