# Library Design

## Design Principle

The library separates three concerns:

- XML/domain model
- protobuf wire DTOs
- mapping and framing logic

## Main Components

- XML/domain model: [`Event.cs`](../dpp.cot/Event.cs), [`Detail.cs`](../dpp.cot/Detail.cs), related typed elements
- protobuf DTOs: [`ProtoContracts.cs`](../dpp.cot/ProtoContracts.cs)
- XML/protobuf mapping: [`CotProtoMapper.cs`](../dpp.cot/CotProtoMapper.cs)
- mesh-style payload framing: [`Message.cs`](../dpp.cot/Message.cs)
- streaming framing: [`TakProtocolStreaming.cs`](../dpp.cot/TakProtocolStreaming.cs)

## Why This Separation Exists

The XML wire format and protobuf wire format are both formal protocols, but they are not structurally identical.

Separating them reduces the risk that:

- XML object shapes accidentally define protobuf behavior
- protobuf field numbering leaks into XML-domain types
- one wire format silently corrupts the other

## Target Frameworks

The library currently multi-targets:

- `netstandard2.0` for broad .NET ecosystem compatibility
- `net10.0` for a current runtime target verified in this repo

The test project remains on the newest local runtime so the library can be validated in this environment while still shipping a broadly consumable package surface.
