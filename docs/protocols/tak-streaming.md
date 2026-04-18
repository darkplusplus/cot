# TAK Streaming Envelope

## Normative Source

The streaming framing described here comes from the public protocol text committed in this repo:

- [`dpp.cot/protobuf/protocol.txt`](../../dpp.cot/protobuf/protocol.txt)

## Envelope Shape

The streaming envelope is:

- one magic byte `0xbf`
- an unsigned protobuf-style varint message length
- the payload bytes

The payload is version-specific and does not contain the mesh-style version-identification header.

## Implementation Support

This repo provides helpers for:

- encoding streaming messages
- parsing a single streaming message
- incrementally parsing one message from a concatenated byte buffer

Implementation Note:
The streaming helpers are implemented in [`dpp.cot/TakProtocolStreaming.cs`](../../dpp.cot/TakProtocolStreaming.cs) and covered by [`dpp.cot.Tests/StreamingTests.cs`](../../dpp.cot.Tests/StreamingTests.cs).
