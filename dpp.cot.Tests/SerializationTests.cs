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
            Assert.Equal(0.0, evt.Detail.Track.Speed);
            Assert.Equal(0.0, evt.Detail.Track.Course);
            Assert.Single(evt.Detail.AdditionalElements);
            Assert.Equal("uid", evt.Detail.AdditionalElements[0].Name);
        }

        [Fact]
        public void MixedDetailPayloadPreservesUnknownElements()
        {
            var evt = Event.Parse(Helpers.MixedDetailPayload);

            Assert.NotNull(evt.Detail);
            Assert.NotNull(evt.Detail.Track);
            Assert.Equal(123.4, evt.Detail.Track.Speed);
            Assert.Equal(270.0, evt.Detail.Track.Course);
            Assert.Single(evt.Detail.AdditionalElements);
            Assert.Equal("uid", evt.Detail.AdditionalElements[0].Name);
            Assert.Contains("Droid", evt.Detail.AdditionalElements[0].OuterXml);
        }

        [Fact]
        public void TakyTakUserPayloadPreservesExtraTypedAttributes()
        {
            var evt = Event.Parse(Helpers.TakyTakUserPayload);

            Assert.NotNull(evt.Detail);
            Assert.NotNull(evt.Detail.Contact);
            Assert.Equal("JENNY", evt.Detail.Contact.Callsign);
            Assert.Equal("*:-1:stcp", evt.Detail.Contact.Endpoint);
            Assert.Single(evt.Detail.Contact.AdditionalAttributes);
            Assert.Equal("phone", evt.Detail.Contact.AdditionalAttributes[0].Name);
            Assert.Equal("800-867-5309", evt.Detail.Contact.AdditionalAttributes[0].Value);
            Assert.Single(evt.Detail.AdditionalElements);
            Assert.Equal("uid", evt.Detail.AdditionalElements[0].Name);
        }

        [Fact]
        public void TakyGeoChatPayloadPreservesUnknownDetailElements()
        {
            var evt = Event.Parse(Helpers.TakyGeoChatPayload);

            Assert.NotNull(evt.Detail);
            Assert.Equal(5, evt.Detail.AdditionalElements.Length);
            Assert.Equal("__chat", evt.Detail.AdditionalElements[0].Name);
            Assert.Equal("link", evt.Detail.AdditionalElements[1].Name);
            Assert.Equal("remarks", evt.Detail.AdditionalElements[2].Name);
            Assert.Equal("__serverdestination", evt.Detail.AdditionalElements[3].Name);
            Assert.Equal("marti", evt.Detail.AdditionalElements[4].Name);
            Assert.Contains("chatgrp", evt.Detail.AdditionalElements[0].InnerXml);
            Assert.Equal("test", evt.Detail.AdditionalElements[2].InnerText);
        }

        [Fact]
        public void AtakMarkerFixturePreservesTypedAndUnknownDetailData()
        {
            var evt = Event.Parse(Helpers.AtakMarker2525Payload);

            Assert.NotNull(evt.Detail);
            Assert.NotNull(evt.Detail.Status);
            Assert.Single(evt.Detail.Status.AdditionalAttributes);
            Assert.Equal("readiness", evt.Detail.Status.AdditionalAttributes[0].Name);
            Assert.Equal("true", evt.Detail.Status.AdditionalAttributes[0].Value);
            Assert.NotNull(evt.Detail.Contact);
            Assert.Equal("U.16.135057", evt.Detail.Contact.Callsign);
            Assert.NotNull(evt.Detail.PrecisionLocation);
            Assert.Equal("???", evt.Detail.PrecisionLocation.Altsrc);
            Assert.Equal(6, evt.Detail.AdditionalElements.Length);
            Assert.Equal("archive", evt.Detail.AdditionalElements[0].Name);
            Assert.Equal("link", evt.Detail.AdditionalElements[1].Name);
            Assert.Equal("archive", evt.Detail.AdditionalElements[3].Name);
            Assert.Equal("usericon", evt.Detail.AdditionalElements[5].Name);
        }

        [Fact]
        public void AtakGeoFenceFixturePreservesNestedUnknownDetailTrees()
        {
            var evt = Event.Parse(Helpers.AtakGeoFencePayload);

            Assert.NotNull(evt.Detail);
            Assert.NotNull(evt.Detail.Contact);
            Assert.Equal("Geo Fence Circle", evt.Detail.Contact.Callsign);
            Assert.NotNull(evt.Detail.PrecisionLocation);
            Assert.Equal("???", evt.Detail.PrecisionLocation.Altsrc);
            Assert.Equal(8, evt.Detail.AdditionalElements.Length);
            Assert.Equal("shape", evt.Detail.AdditionalElements[0].Name);
            Assert.Contains("ellipse", evt.Detail.AdditionalElements[0].InnerXml);
            Assert.Equal("__geofence", evt.Detail.AdditionalElements[1].Name);
            Assert.Equal("archive", evt.Detail.AdditionalElements[6].Name);
            Assert.Equal("labels_on", evt.Detail.AdditionalElements[7].Name);
        }

        [Fact]
        public void AtakRouteFixturePreservesRepeatedOrderedUnknownElements()
        {
            var evt = Event.Parse(Helpers.AtakRoutePayload);

            Assert.NotNull(evt.Detail);
            Assert.NotNull(evt.Detail.Contact);
            Assert.Equal("Route 1", evt.Detail.Contact.Callsign);
            Assert.Equal(21, evt.Detail.AdditionalElements.Length);
            Assert.Equal("link", evt.Detail.AdditionalElements[0].Name);
            Assert.Equal("link", evt.Detail.AdditionalElements[12].Name);
            Assert.Equal("link_attr", evt.Detail.AdditionalElements[13].Name);
            Assert.Equal("__routeinfo", evt.Detail.AdditionalElements[16].Name);
            Assert.Contains("__navcues", evt.Detail.AdditionalElements[16].InnerXml);
            Assert.Equal("remarks", evt.Detail.AdditionalElements[17].Name);
            Assert.Equal("archive", evt.Detail.AdditionalElements[18].Name);
            Assert.Equal("labels_on", evt.Detail.AdditionalElements[19].Name);
            Assert.Equal("color", evt.Detail.AdditionalElements[20].Name);
        }

        [Theory]
        [InlineData(Helpers.SimplePayload)]
        [InlineData(Helpers.EudPayload)]
        [InlineData(Helpers.MixedDetailPayload)]
        [InlineData(Helpers.TakyTakUserPayload)]
        [InlineData(Helpers.TakyGeoChatPayload)]
        [InlineData(Helpers.AtakMarker2525Payload)]
        [InlineData(Helpers.AtakGeoFencePayload)]
        [InlineData(Helpers.AtakRoutePayload)]
        public void XmlRoundTripPreservesSemantics(string corpus)
        {
            var original = Event.Parse(corpus);
            var xml = original.ToXmlString();
            var roundTrip = Event.Parse(xml);

            Assert.DoesNotContain("xmlns", xml);
            Assert.DoesNotContain("w3.org", xml);

            AssertEventsEquivalent(original, roundTrip);
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

        private static void AssertEventsEquivalent(Event expected, Event actual)
        {
            Assert.Equal(expected.Version, actual.Version);
            Assert.Equal(expected.Uid, actual.Uid);
            Assert.Equal(expected.Type, actual.Type);
            Assert.Equal(expected.Access, actual.Access);
            Assert.Equal(expected.Qos, actual.Qos);
            Assert.Equal(expected.Opex, actual.Opex);
            Assert.Equal(expected.How, actual.How);
            Assert.Equal(expected.Time, actual.Time);
            Assert.Equal(expected.Start, actual.Start);
            Assert.Equal(expected.Stale, actual.Stale);
            Assert.Equal(expected.Point.Lat, actual.Point.Lat);
            Assert.Equal(expected.Point.Lon, actual.Point.Lon);
            Assert.Equal(expected.Point.Hae, actual.Point.Hae);
            Assert.Equal(expected.Point.Ce, actual.Point.Ce);
            Assert.Equal(expected.Point.Le, actual.Point.Le);

            if (expected.Detail == null)
            {
                Assert.Null(actual.Detail);
                return;
            }

            Assert.NotNull(actual.Detail);

            Assert.Equal(expected.Detail.Contact?.Endpoint, actual.Detail.Contact?.Endpoint);
            Assert.Equal(expected.Detail.Contact?.Callsign, actual.Detail.Contact?.Callsign);
            AssertXmlAttributesEquivalent(expected.Detail.Contact?.AdditionalAttributes, actual.Detail.Contact?.AdditionalAttributes);
            Assert.Equal(expected.Detail.Group?.Name, actual.Detail.Group?.Name);
            Assert.Equal(expected.Detail.Group?.Role, actual.Detail.Group?.Role);
            AssertXmlAttributesEquivalent(expected.Detail.Group?.AdditionalAttributes, actual.Detail.Group?.AdditionalAttributes);
            Assert.Equal(expected.Detail.PrecisionLocation?.Geopointsrc, actual.Detail.PrecisionLocation?.Geopointsrc);
            Assert.Equal(expected.Detail.PrecisionLocation?.Altsrc, actual.Detail.PrecisionLocation?.Altsrc);
            AssertXmlAttributesEquivalent(expected.Detail.PrecisionLocation?.AdditionalAttributes, actual.Detail.PrecisionLocation?.AdditionalAttributes);
            Assert.Equal(expected.Detail.Status?.Battery, actual.Detail.Status?.Battery);
            AssertXmlAttributesEquivalent(expected.Detail.Status?.AdditionalAttributes, actual.Detail.Status?.AdditionalAttributes);
            Assert.Equal(expected.Detail.Takv?.Device, actual.Detail.Takv?.Device);
            Assert.Equal(expected.Detail.Takv?.Platform, actual.Detail.Takv?.Platform);
            Assert.Equal(expected.Detail.Takv?.Os, actual.Detail.Takv?.Os);
            Assert.Equal(expected.Detail.Takv?.Version, actual.Detail.Takv?.Version);
            AssertXmlAttributesEquivalent(expected.Detail.Takv?.AdditionalAttributes, actual.Detail.Takv?.AdditionalAttributes);
            Assert.Equal(expected.Detail.Track?.Speed, actual.Detail.Track?.Speed);
            Assert.Equal(expected.Detail.Track?.Course, actual.Detail.Track?.Course);
            AssertXmlAttributesEquivalent(expected.Detail.Track?.AdditionalAttributes, actual.Detail.Track?.AdditionalAttributes);

            var expectedAdditional = expected.Detail.AdditionalElements ?? Array.Empty<System.Xml.XmlElement>();
            var actualAdditional = actual.Detail.AdditionalElements ?? Array.Empty<System.Xml.XmlElement>();
            Assert.Equal(expectedAdditional.Length, actualAdditional.Length);

            for (var i = 0; i < expectedAdditional.Length; i++)
            {
                Assert.Equal(expectedAdditional[i].OuterXml, actualAdditional[i].OuterXml);
            }
        }

        private static void AssertXmlAttributesEquivalent(System.Xml.XmlAttribute[] expected, System.Xml.XmlAttribute[] actual)
        {
            var expectedAttributes = expected ?? Array.Empty<System.Xml.XmlAttribute>();
            var actualAttributes = actual ?? Array.Empty<System.Xml.XmlAttribute>();

            Assert.Equal(expectedAttributes.Length, actualAttributes.Length);

            for (var i = 0; i < expectedAttributes.Length; i++)
            {
                Assert.Equal(expectedAttributes[i].Name, actualAttributes[i].Name);
                Assert.Equal(expectedAttributes[i].Value, actualAttributes[i].Value);
            }
        }
    }
}
