using ProtoBuf;
using System;
using System.IO;
using Xunit;

namespace dpp.cot.Tests
{
    public class ProtoMappingTests
    {
        [Fact]
        public void ProtobufPayloadUsesAuthoritativeWireShape()
        {
            var evt = Event.Parse(Helpers.MixedDetailPayload);
            evt.Caveat = "TEST-CAVEAT";
            evt.ReleaseableTo = "USA";
            var message = new Message
            {
                Event = evt,
                Control = new TakControl
                {
                    minProtoVersion = 1,
                    maxProtoVersion = 1,
                },
                SubmissionTime = new DateTime(2022, 2, 2, 23, 0, 0, DateTimeKind.Utc),
                CreationTime = new DateTime(2022, 2, 2, 23, 5, 0, DateTimeKind.Utc),
            };

            var framed = message.ToProtobufBytes();

            Assert.Equal(0xef, framed[0]);
            Assert.Equal(0x01, framed[1]);
            Assert.Equal(0xef, framed[2]);

            using var ms = new MemoryStream(framed, 3, framed.Length - 3, writable: false);
            var proto = Serializer.Deserialize<ProtoTakMessage>(ms);

            Assert.NotNull(proto.TakControl);
            Assert.Equal((uint)1, proto.TakControl.MinProtoVersion);
            Assert.Equal((uint)1, proto.TakControl.MaxProtoVersion);

            Assert.NotNull(proto.CotEvent);
            Assert.Equal("MIXED-1", proto.CotEvent.Uid);
            Assert.Equal("a-f-G-U-C", proto.CotEvent.Type);
            Assert.Equal("h-e", proto.CotEvent.How);
            Assert.Equal("TEST-CAVEAT", proto.CotEvent.Caveat);
            Assert.Equal("USA", proto.CotEvent.ReleaseableTo);
            Assert.Equal(1.0, proto.CotEvent.Lat);
            Assert.Equal(2.0, proto.CotEvent.Lon);
            Assert.Equal(3.0, proto.CotEvent.Hae);
            Assert.Equal(4.0, proto.CotEvent.Ce);
            Assert.Equal(5.0, proto.CotEvent.Le);
            Assert.Equal((ulong)new DateTimeOffset(evt.Time).ToUnixTimeMilliseconds(), proto.CotEvent.SendTime);
            Assert.Equal((ulong)new DateTimeOffset(evt.Start).ToUnixTimeMilliseconds(), proto.CotEvent.StartTime);
            Assert.Equal((ulong)new DateTimeOffset(evt.Stale).ToUnixTimeMilliseconds(), proto.CotEvent.StaleTime);
            Assert.Equal((ulong)new DateTimeOffset(message.SubmissionTime.Value).ToUnixTimeMilliseconds(), proto.SubmissionTime);
            Assert.Equal((ulong)new DateTimeOffset(message.CreationTime.Value).ToUnixTimeMilliseconds(), proto.CreationTime);

            Assert.NotNull(proto.CotEvent.Detail);
            Assert.NotNull(proto.CotEvent.Detail.Contact);
            Assert.NotNull(proto.CotEvent.Detail.Track);
            Assert.Equal("ALPHA", proto.CotEvent.Detail.Contact.Callsign);
            Assert.Equal(123.4, proto.CotEvent.Detail.Track.Speed);
            Assert.Equal(270.0, proto.CotEvent.Detail.Track.Course);
            Assert.Contains("<uid Droid=\"ALPHA\" />", proto.CotEvent.Detail.XmlDetail);
            Assert.DoesNotContain("<contact", proto.CotEvent.Detail.XmlDetail);
            Assert.DoesNotContain("<track", proto.CotEvent.Detail.XmlDetail);
        }

        [Fact]
        public void ProtoDetailResidualXmlRehydratesAdditionalElements()
        {
            var message = new ProtoTakMessage
            {
                TakControl = new ProtoTakControl
                {
                    MinProtoVersion = 1,
                    MaxProtoVersion = 1,
                },
                CotEvent = new ProtoCotEvent
                {
                    Uid = "PROTO-1",
                    Type = "a-f-G-U-C",
                    How = "h-e",
                    SendTime = (ulong)new DateTimeOffset(2022, 2, 2, 22, 22, 22, TimeSpan.Zero).ToUnixTimeMilliseconds(),
                    StartTime = (ulong)new DateTimeOffset(2022, 2, 2, 22, 22, 22, TimeSpan.Zero).ToUnixTimeMilliseconds(),
                    StaleTime = (ulong)new DateTimeOffset(2022, 2, 2, 22, 32, 22, TimeSpan.Zero).ToUnixTimeMilliseconds(),
                    Lat = 10.0,
                    Lon = 20.0,
                    Hae = 30.0,
                    Ce = 40.0,
                    Le = 50.0,
                    Caveat = "CAVEAT",
                    ReleaseableTo = "REL",
                    Detail = new ProtoDetail
                    {
                        XmlDetail = "<uid Droid=\"PROTO-1\" /><remarks source=\"test\">hello</remarks>",
                        Track = new Track
                        {
                            Speed = 55.5,
                            Course = 180.0,
                        }
                    }
                }
            };

            var hydrated = CotProtoMapper.FromProto(message);

            Assert.NotNull(hydrated.Event);
            Assert.NotNull(hydrated.Event.Detail);
            Assert.NotNull(hydrated.Event.Detail.Track);
            Assert.Equal("CAVEAT", hydrated.Event.Caveat);
            Assert.Equal("REL", hydrated.Event.ReleaseableTo);
            Assert.Equal(55.5, hydrated.Event.Detail.Track.Speed);
            Assert.Equal(180.0, hydrated.Event.Detail.Track.Course);
            Assert.Equal(2, hydrated.Event.Detail.AdditionalElements.Length);
            Assert.Equal("uid", hydrated.Event.Detail.AdditionalElements[0].Name);
            Assert.Equal("remarks", hydrated.Event.Detail.AdditionalElements[1].Name);
            Assert.Equal("hello", hydrated.Event.Detail.AdditionalElements[1].InnerText);
        }

        [Fact]
        public void XmlDetailOverridesTypedDetailElementsOnDecode()
        {
            var message = new ProtoTakMessage
            {
                CotEvent = new ProtoCotEvent
                {
                    Uid = "OVERRIDE-1",
                    Type = "a-f-G-U-C",
                    How = "h-e",
                    SendTime = (ulong)new DateTimeOffset(2022, 2, 2, 22, 22, 22, TimeSpan.Zero).ToUnixTimeMilliseconds(),
                    StartTime = (ulong)new DateTimeOffset(2022, 2, 2, 22, 22, 22, TimeSpan.Zero).ToUnixTimeMilliseconds(),
                    StaleTime = (ulong)new DateTimeOffset(2022, 2, 2, 22, 32, 22, TimeSpan.Zero).ToUnixTimeMilliseconds(),
                    Lat = 1,
                    Lon = 2,
                    Hae = 3,
                    Ce = 4,
                    Le = 5,
                    Detail = new ProtoDetail
                    {
                        Contact = new Contact
                        {
                            Callsign = "TYPED",
                            Endpoint = "1.2.3.4:1111:tcp"
                        },
                        XmlDetail = "<contact callsign=\"XML\" endpoint=\"2.3.4.5:2222:tcp\" />"
                    }
                }
            };

            var hydrated = CotProtoMapper.FromProto(message);

            Assert.Null(hydrated.Event.Detail.Contact);
            Assert.Single(hydrated.Event.Detail.AdditionalElements);
            Assert.Equal("contact", hydrated.Event.Detail.AdditionalElements[0].Name);
            Assert.Contains("callsign=\"XML\"", hydrated.Event.Detail.AdditionalElements[0].OuterXml);
        }

        [Fact]
        public void KnownTypedDetailWithExtraAttributesFallsBackToXmlDetail()
        {
            var evt = Event.Parse(Helpers.TakyTakUserPayload);
            var message = new Message { Event = evt };

            var framed = message.ToProtobufBytes();

            using var ms = new MemoryStream(framed, 3, framed.Length - 3, writable: false);
            var proto = Serializer.Deserialize<ProtoTakMessage>(ms);

            Assert.NotNull(proto.CotEvent);
            Assert.NotNull(proto.CotEvent.Detail);
            Assert.Null(proto.CotEvent.Detail.Contact);
            Assert.Contains("<contact", proto.CotEvent.Detail.XmlDetail);
            Assert.Contains("phone=\"800-867-5309\"", proto.CotEvent.Detail.XmlDetail);
            Assert.NotNull(proto.CotEvent.Detail.Track);
            Assert.NotNull(proto.CotEvent.Detail.Takv);
        }
    }
}
