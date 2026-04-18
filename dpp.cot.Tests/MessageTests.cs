using System;
using System.Linq;
using Xunit;

namespace dpp.cot.Tests
{
    public class MessageTests
    {
        [Fact]
        public void ParseFramedXmlRespectsOffsetAndLength()
        {
            var evt = Event.Parse(Helpers.MixedDetailPayload);
            var message = new Message
            {
                Event = evt,
                Control = new TakControl
                {
                    minProtoVersion = 0,
                    maxProtoVersion = 0,
                }
            };

            var framed = message.ToXmlBytes();
            var prefix = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            var suffix = new byte[] { 0x05, 0x06 };
            var buffer = prefix.Concat(framed).Concat(suffix).ToArray();

            var parsed = Message.Parse(buffer, prefix.Length, framed.Length);

            Assert.Equal("MIXED-1", parsed.Event.Uid);
            Assert.Equal(123.4, parsed.Event.Detail.Track.Speed);
            Assert.Single(parsed.Event.Detail.AdditionalElements);
        }

        [Fact]
        public void ProtobufRoundTripPreservesResidualDetailAndTrack()
        {
            var evt = Event.Parse(Helpers.MixedDetailPayload);
            var message = new Message
            {
                Event = evt,
                Control = new TakControl
                {
                    minProtoVersion = 1,
                    maxProtoVersion = 1,
                },
                SubmissionTime = new DateTime(2022, 2, 3, 0, 0, 0, DateTimeKind.Utc),
                CreationTime = new DateTime(2022, 2, 3, 0, 5, 0, DateTimeKind.Utc),
            };

            var framed = message.ToProtobufBytes();
            var parsed = Message.Parse(framed, 0, framed.Length);

            Assert.Equal("MIXED-1", parsed.Event.Uid);
            Assert.Equal(123.4, parsed.Event.Detail.Track.Speed);
            Assert.Single(parsed.Event.Detail.AdditionalElements);
            Assert.Contains("Droid", parsed.Event.Detail.AdditionalElements[0].OuterXml);
            Assert.Equal((uint)1, parsed.Control.minProtoVersion);
            Assert.Equal((uint)1, parsed.Control.maxProtoVersion);
            Assert.Equal(message.SubmissionTime, parsed.SubmissionTime);
            Assert.Equal(message.CreationTime, parsed.CreationTime);
        }
    }
}
