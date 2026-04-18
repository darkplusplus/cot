# CoT XML Event

## Scope

This page describes the CoT XML `event` shape supported by `dpp.cot`.

## Public Basis

- the public MITRE CoT overview linked in [Sources](../sources.md)
- repo-local public artifacts such as [`dpp.cot/CoTtypes.xml`](../../dpp.cot/CoTtypes.xml)

For type-string lineage and other compact taxonomy inferences, see [Type Inference](../type-inference.md).

## Supported Base Shape

`dpp.cot` models the XML event as:

- `event` attributes such as `version`, `uid`, `type`, `time`, `start`, `stale`, and `how`
- a `point` child
- an optional `detail` child

## Detail Handling

Known typed `detail` children currently modeled in the library include:

- `contact`
- `__group`
- `precisionlocation`
- `status`
- `takv`
- `track`

Unknown `detail` children are preserved as residual XML.

Implementation Note:
This preservation behavior is backed by tests in [`dpp.cot.Tests/SerializationTests.cs`](../../dpp.cot.Tests/SerializationTests.cs).

## Time Handling

The XML serializer emits UTC timestamps.

Implementation Note:
The serializer behavior is implemented in [`dpp.cot/Event.cs`](../../dpp.cot/Event.cs).
