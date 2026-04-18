# Testing

## Principle

Claims in the docs should correspond to executable checks in the test suite whenever practical.

## Current Coverage Areas

- XML parsing and semantic round-trip
- known and unknown `detail` preservation
- extra attributes on known typed `detail` elements
- protobuf payload mapping
- mesh-style framed payload handling
- streaming envelope handling
- CoT predicate and description helpers

## Public Fixture Sources

- [`takcot/examples/Marker - 2525.cot` in `deptofdefense/AndroidTacticalAssaultKit-CIV`](https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV/blob/main/takcot/examples/Marker%20-%202525.cot)
- [`takcot/examples/Geo Fence.cot` in `deptofdefense/AndroidTacticalAssaultKit-CIV`](https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV/blob/main/takcot/examples/Geo%20Fence.cot)
- [`takcot/examples/Route.cot` in `deptofdefense/AndroidTacticalAssaultKit-CIV`](https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV/blob/main/takcot/examples/Route.cot)
- [`tests/test_takuser.py` in `tkuester/taky`](https://github.com/tkuester/taky/blob/016f2c3657455617dae3c302cef7ca0c743df8e4/tests/test_takuser.py)
- [`tests/test_geo_chat.py` in `tkuester/taky`](https://github.com/tkuester/taky/blob/016f2c3657455617dae3c302cef7ca0c743df8e4/tests/test_geo_chat.py)
- [`tests/test_classes.py` in `snstac/pytak`](https://github.com/snstac/pytak/blob/fa875a181bd724a7a841be2384677fc363ebbac5/tests/test_classes.py)
- [`tests/test_functions.py` in `snstac/pytak`](https://github.com/snstac/pytak/blob/fa875a181bd724a7a841be2384677fc363ebbac5/tests/test_functions.py)

These are public fixture and behavior references, not normative specifications. When a payload is borrowed from a public test, the originating file should be cited in the corresponding local test or documentation note.

The ATAK CIV example fixtures are especially useful because they exercise real client-authored `detail` trees with repeated elements, nested subtrees, and non-protobuf detail extensions.

## Test Files

- [`SerializationTests.cs`](../dpp.cot.Tests/SerializationTests.cs)
- [`MessageTests.cs`](../dpp.cot.Tests/MessageTests.cs)
- [`ProtoMappingTests.cs`](../dpp.cot.Tests/ProtoMappingTests.cs)
- [`StreamingTests.cs`](../dpp.cot.Tests/StreamingTests.cs)
- [`CotTypeTests.cs`](../dpp.cot.Tests/CotTypeTests.cs)

## Documentation Rule

If a new protocol behavior is documented as supported, it should normally gain:

1. a cited source or an explicit implementation note
2. a focused test
3. a code path that can be pointed to directly
