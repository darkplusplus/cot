# Fixture Provenance

This page records the public origin of borrowed or adapted protocol fixtures used in the test suite.

## Rule

When a test fixture is adapted from a public upstream source, the local fixture should:

- name the upstream repository
- point to the upstream file when practical
- explain what protocol behavior the fixture protects

## ATAK CIV Fixtures

Source repository:
- <https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV>

Imported example files:
- [`takcot/examples/Marker - 2525.cot`](https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV/blob/main/takcot/examples/Marker%20-%202525.cot)
- [`takcot/examples/Geo Fence.cot`](https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV/blob/main/takcot/examples/Geo%20Fence.cot)
- [`takcot/examples/Route.cot`](https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV/blob/main/takcot/examples/Route.cot)

Local use:
- [`Helpers.cs`](../dpp.cot.Tests/Helpers.cs)
- [`SerializationTests.cs`](../dpp.cot.Tests/SerializationTests.cs)

Behaviors protected:
- repeated unknown `detail` elements remain ordered
- nested unknown `detail` XML subtrees are preserved
- common ATAK client payloads survive XML parse and semantic round-trip
- typed elements with extra XML attributes are not silently flattened away

## taky Fixtures

Source repository:
- <https://github.com/tkuester/taky>

Imported test files:
- [`tests/test_takuser.py`](https://github.com/tkuester/taky/blob/016f2c3657455617dae3c302cef7ca0c743df8e4/tests/test_takuser.py)
- [`tests/test_geo_chat.py`](https://github.com/tkuester/taky/blob/016f2c3657455617dae3c302cef7ca0c743df8e4/tests/test_geo_chat.py)

Local use:
- [`Helpers.cs`](../dpp.cot.Tests/Helpers.cs)
- [`SerializationTests.cs`](../dpp.cot.Tests/SerializationTests.cs)
- [`ProtoMappingTests.cs`](../dpp.cot.Tests/ProtoMappingTests.cs)

Behaviors protected:
- extra attributes on typed `detail` elements
- GeoChat-style residual XML preservation
- protobuf `xmlDetail` fallback for typed elements that no longer fit the proto shape

## PyTAK References

Source repository:
- <https://github.com/snstac/pytak>

Referenced test files:
- [`tests/test_classes.py`](https://github.com/snstac/pytak/blob/fa875a181bd724a7a841be2384677fc363ebbac5/tests/test_classes.py)
- [`tests/test_functions.py`](https://github.com/snstac/pytak/blob/fa875a181bd724a7a841be2384677fc363ebbac5/tests/test_functions.py)

Current use:
- documentation and behavior reference only

Behaviors observed:
- generated CoT event defaults
- common event and point attribute expectations
- `_flow-tags_` detail output

## Provenance Boundary

Fixtures here are used as public interoperability examples, not as normative specification text.

The normative hierarchy for this repo remains:

1. local copies of public protocol artifacts such as `.proto`, `protocol.txt`, and `takcot` XSD material
2. public implementation references such as ATAK CIV and TAK Server
3. public fixture sources used to protect specific tested behaviors
