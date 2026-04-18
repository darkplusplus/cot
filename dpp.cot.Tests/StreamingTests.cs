using System.Linq;
using System.Text;
using Xunit;

namespace dpp.cot.Tests
{
    public class StreamingTests
    {
        [Fact]
        public void XmlStreamingRoundTripPreservesMessage()
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

            var streamed = message.ToStreamingBytes(0x00);
            var parsed = Message.ParseStreaming(streamed, 0, streamed.Length, 0x00);

            Assert.Equal("MIXED-1", parsed.Event.Uid);
            Assert.Equal(123.4, parsed.Event.Detail.Track.Speed);
            Assert.Single(parsed.Event.Detail.AdditionalElements);
        }

        [Fact]
        public void ProtobufStreamingRoundTripPreservesMessage()
        {
            var evt = Event.Parse(Helpers.MixedDetailPayload);
            var message = new Message
            {
                Event = evt,
                Control = new TakControl
                {
                    minProtoVersion = 1,
                    maxProtoVersion = 1,
                }
            };

            var streamed = message.ToStreamingBytes(0x01);
            var parsed = Message.ParseStreaming(streamed, 0, streamed.Length, 0x01);

            Assert.Equal("MIXED-1", parsed.Event.Uid);
            Assert.Equal((uint)1, parsed.Control.minProtoVersion);
            Assert.Single(parsed.Event.Detail.AdditionalElements);
        }

        [Fact]
        public void TryParseStreamingConsumesOnlyOneMessageFromConcatenatedBuffer()
        {
            var first = new Message
            {
                Event = Event.Parse(Helpers.MixedDetailPayload),
                Control = new TakControl
                {
                    minProtoVersion = 1,
                    maxProtoVersion = 1,
                }
            };
            var second = new Message
            {
                Event = Event.Parse(Helpers.EudPayload),
                Control = new TakControl
                {
                    minProtoVersion = 1,
                    maxProtoVersion = 1,
                }
            };

            var firstBytes = first.ToStreamingBytes(0x01);
            var secondBytes = second.ToStreamingBytes(0x01);
            var buffer = firstBytes.Concat(secondBytes).ToArray();

            Assert.True(Message.TryParseStreaming(buffer, 0, buffer.Length, 0x01, out var parsedFirst, out var firstConsumed));
            Assert.Equal(firstBytes.Length, firstConsumed);
            Assert.Equal("MIXED-1", parsedFirst.Event.Uid);

            Assert.True(Message.TryParseStreaming(buffer, firstConsumed, buffer.Length - firstConsumed, 0x01, out var parsedSecond, out var secondConsumed));
            Assert.Equal(secondBytes.Length, secondConsumed);
            Assert.Equal("ANDROID-ASDFASDFASDF", parsedSecond.Event.Uid);
        }

        [Fact]
        public void StreamingHeaderSupportsMultiByteVarintLengths()
        {
            var largeRemark = new string('x', 300);
            var largePayload =
                "<event version='2.0' uid='STREAM-LARGE' type='a-f-G-U-C' time='2022-02-02T22:22:22.222Z' start='2022-02-02T22:22:22.222Z' stale='2022-02-02T22:32:22.222Z' how='h-e'>" +
                "<point lat='1.0' lon='2.0' hae='3.0' ce='4.0' le='5.0' />" +
                $"<detail><remarks source='test'>{largeRemark}</remarks></detail></event>";

            var message = new Message
            {
                Event = Event.Parse(largePayload),
                Control = new TakControl
                {
                    minProtoVersion = 0,
                    maxProtoVersion = 0,
                }
            };

            var streamed = message.ToStreamingBytes(0x00);

            Assert.Equal(0xbf, streamed[0]);
            Assert.True((streamed[1] & 0x80) != 0);

            var parsed = Message.ParseStreaming(streamed, 0, streamed.Length, 0x00);

            Assert.Equal("STREAM-LARGE", parsed.Event.Uid);
            Assert.Single(parsed.Event.Detail.AdditionalElements);
            Assert.Contains(largeRemark, parsed.Event.Detail.AdditionalElements[0].OuterXml);
        }
    }
}
