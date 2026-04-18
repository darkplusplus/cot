# Interoperability Notes

This page records public implementation-reference observations relevant to interoperability.

## Public Implementation Reference Used Here

- ATAK CIV client repository: <https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV>
- TAK Product Center Server repository: <https://github.com/TAK-Product-Center/Server>
- PyTAK repository: <https://github.com/snstac/pytak>
- taky repository: <https://github.com/tkuester/taky>
- FreeTakServer repository: <https://github.com/FreeTAKTeam/FreeTakServer>

The ATAK CIV client and TAK Server repositories are treated as the primary public implementation references here, not as replacements for normative protocol artifacts.

## Current Compatibility Notes

### TAK Protobuf `TakMessage`

The public TAK Server protobuf definition includes:

- `takControl = 1`
- `cotEvent = 2`
- `submissionTime = 3`
- `creationTime = 4`

Source:
- [`takmessage.proto` in TAK Server](https://github.com/TAK-Product-Center/Server/blob/5187abd46d827d37cfc5708805eced197a837e49/src/takserver-protobuf/src/main/proto/takmessage.proto)

Current `dpp.cot` status:

- supports `takControl`
- supports `cotEvent`
- supports `submissionTime`
- supports `creationTime`

Compatibility implication:

- these top-level protobuf timestamps now round-trip through the current `dpp.cot` object model

### TAK Protobuf `TakControl`

The public TAK Server protobuf definition for `TakControl` contains:

- `minProtoVersion`
- `maxProtoVersion`

Source:
- [`takcontrol.proto` in TAK Server](https://github.com/TAK-Product-Center/Server/blob/5187abd46d827d37cfc5708805eced197a837e49/src/takserver-protobuf/src/main/proto/takcontrol.proto)

Current `dpp.cot` status:

- includes `minProtoVersion`
- includes `maxProtoVersion`
- does not include non-public or extra `TakControl` protobuf fields beyond the public TAK Server shape

Compatibility implication:

- `TakControl` is now aligned with the public TAK Server protobuf definition referenced above

### TAK Protobuf `CotEvent`

The public TAK Server protobuf definition includes optional `caveat` and `releaseableTo` fields in addition to the event fields already implemented here.

Source:
- [`cotevent.proto` in TAK Server](https://github.com/TAK-Product-Center/Server/blob/5187abd46d827d37cfc5708805eced197a837e49/src/takserver-protobuf/src/main/proto/cotevent.proto)

Current `dpp.cot` status:

- supports `type`, `access`, `qos`, `opex`, `caveat`, `releaseableTo`, `uid`, `sendTime`, `startTime`, `staleTime`, `how`, point fields, and `detail`

Compatibility implication:

- these optional fields now round-trip through the current `dpp.cot` implementation

### Mesh-Style Protobuf Header

The public TAK Server `SingleProtobufOrCotProtocol` reads the mesh-style protobuf header as:

- magic byte
- unsigned varint protocol version
- magic byte
- protobuf payload

Source:
- [`SingleProtobufOrCotProtocol.java` in TAK Server](https://github.com/TAK-Product-Center/Server/blob/5187abd46d827d37cfc5708805eced197a837e49/src/takserver-core/src/main/java/com/bbn/marti/nio/protocol/connections/SingleProtobufOrCotProtocol.java)

Current `dpp.cot` status:

- supports the same visible version-1 byte sequence for current use
- parses mesh-style protocol version as an unsigned varint

Compatibility implication:

- current version-1 compatibility is aligned and the framing implementation matches the public varint-based version parsing model

### Streaming Envelope

The public TAK Server streaming implementation uses:

- `0xbf` magic byte
- unsigned varint payload length
- payload bytes

Sources:
- [`StreamingProtoBufProtocol.java` in TAK Server](https://github.com/TAK-Product-Center/Server/blob/5187abd46d827d37cfc5708805eced197a837e49/src/takserver-core/src/main/java/com/bbn/marti/nio/protocol/connections/StreamingProtoBufProtocol.java)
- [`StreamingProtoBufHelper.java` in TAK Server](https://github.com/TAK-Product-Center/Server/blob/5187abd46d827d37cfc5708805eced197a837e49/src/takserver-plugins/src/main/java/tak/server/proto/StreamingProtoBufHelper.java)

Current `dpp.cot` status:

- implements the same streaming envelope shape
- supports chained-message parsing from concatenated buffers

### `detail` Conversion Rule Strictness

The public TAK Server protobuf helper only promotes supported `detail` child elements into typed protobuf fields when the whole element matches the expected attribute set. Otherwise, the element remains in `xmlDetail`.

Source:
- [`StreamingProtoBufHelper.java` in TAK Server](https://github.com/TAK-Product-Center/Server/blob/5187abd46d827d37cfc5708805eced197a837e49/src/takserver-plugins/src/main/java/tak/server/proto/StreamingProtoBufHelper.java)

Current `dpp.cot` status:

- preserves unknown `detail` child elements
- supports typed mapping for known `detail` child elements
- preserves extra XML attributes on supported typed `detail` elements during XML parse and XML serialization
- falls back to residual `xmlDetail` when a known typed element contains extra attributes that cannot be represented by the protobuf type
- applies `xmlDetail` override semantics on protobuf decode when residual XML contains the same known element names

Compatibility implication:

- receiver-side duplicate-name override behavior is aligned more closely with the public TAK Server implementation
- public fixtures such as the `taky` TAK-user payload can now retain extra attributes in XML and degrade to residual `xmlDetail` on protobuf conversion instead of silently dropping them

### Public Fixture Coverage

Public tests and examples from other implementations are useful when they expose real CoT payloads.

Current examples in this repo include:

- `deptofdefense/AndroidTacticalAssaultKit-CIV` as a primary public client-side interoperability reference
- ATAK CIV example CoT payloads for markers, routes, geofences, and drawing products under `takcot/examples/`
- `tkuester/taky` test fixtures for a TAK-user event with an extra `contact phone=...` attribute and a GeoChat event with `__chat`, `link`, `remarks`, `__serverdestination`, and `marti`
- `snstac/pytak` tests for generated CoT event structure, point defaults, and `_flow-tags_` detail output
- `FreeTakServer` embedded XML samples that show common ATAK-style `detail` combinations

Implication:

- `dpp.cot` must preserve repeated unknown elements, nested unknown XML subtrees, and typed elements that carry extra XML attributes because public ATAK client examples use all three patterns

## Summary

Based on the public ATAK/TAK implementation references, `dpp.cot` appears broadly aligned on:

- CoT XML event basics
- typed `detail` handling for supported elements
- protobuf point flattening and Unix millisecond times
- TAK streaming envelope framing

Known compatibility gaps relative to those public implementations are narrower now and mainly concern unsupported typed detail sub-schemas that are still preserved only as residual XML.
