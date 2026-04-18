# TAK Protobuf Payload

## Normative Source

The protobuf payload v1 mapping used by this repo is defined by the public `.proto` files committed in this repository:

- [`takmessage.proto`](../../dpp.cot/protobuf/takmessage.proto)
- [`cotevent.proto`](../../dpp.cot/protobuf/cotevent.proto)
- [`detail.proto`](../../dpp.cot/protobuf/detail.proto)
- [`track.proto`](../../dpp.cot/protobuf/track.proto)

## Additional Public Implementation Context

For interoperability-oriented context, this repo may also reference public ATAK CIV and TAK Server artifacts listed in [Sources](../sources.md).

Those repositories are treated here as public implementation references, not as the normative source for protobuf field definitions.

When those public sources expose slightly different protobuf surfaces, the rule used by this repo is documented in [Compatibility Policy](../compatibility-policy.md).

## Implementation Approach

This repo uses dedicated protobuf DTOs to match the authoritative wire contract.

The XML/domain model is kept separate from the protobuf DTO layer because:

- the XML shape is hierarchical
- the protobuf shape is flattened in places
- protobuf timestamps use Unix milliseconds

## Mapping Behavior

Current mapping behavior includes:

- `Point` flattening into `lat`, `lon`, `hae`, `ce`, and `le`
- event time conversion to Unix milliseconds
- typed mapping for supported `detail` elements
- residual unknown `detail` XML stored in `xmlDetail`
- whole-element fallback to `xmlDetail` when a known typed element no longer fits the proto shape exactly

Implementation Note:
The mapping code lives in [`dpp.cot/CotProtoMapper.cs`](../../dpp.cot/CotProtoMapper.cs) and is tested in [`dpp.cot.Tests/ProtoMappingTests.cs`](../../dpp.cot.Tests/ProtoMappingTests.cs).
