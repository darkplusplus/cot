# Compatibility Policy

This page describes how `dpp.cot` handles compatibility when public ATAK/TAK sources do not expose exactly the same protobuf surface.

## Source Hierarchy

For protobuf and transport behavior, this repo uses the following hierarchy:

1. repo-local public protocol artifacts committed with this project
2. public ATAK CIV client artifacts
3. public TAK Server artifacts
4. implementation policy documented here and backed by tests

Sources:

- [`dpp.cot/protobuf/`](../dpp.cot/protobuf/)
- <https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV/tree/main/takproto>
- <https://github.com/TAK-Product-Center/Server>

## Baseline

The compatibility baseline for `dpp.cot` is:

- decode legacy XML CoT correctly
- encode and decode protobuf v1 payloads using the committed local `.proto` contract
- preserve XML `detail` information even when it exceeds the protobuf typed subset

This means XML fidelity is treated as a first-class requirement, not as a lossy staging format for protobuf.

## Public Proto Split

The public ATAK CIV client repository and the public TAK Server repository do not expose an identical protobuf surface.

Observed split:

- ATAK CIV `takmessage.proto` exposes `takControl` and `cotEvent`
- public TAK Server `takmessage.proto` also exposes `submissionTime` and `creationTime`
- ATAK CIV `cotevent.proto` exposes the base event fields and `detail`
- public TAK Server `cotevent.proto` also exposes fields such as `caveat` and `releaseableTo`

Implication:

- `dpp.cot` should remain able to interoperate with the narrower public ATAK CIV client surface
- `dpp.cot` may also support public TAK Server extensions when they remain wire-compatible and do not break the client baseline

## Policy For Additional Fields

`dpp.cot` may implement public server-side fields beyond the public ATAK CIV client baseline when all of the following hold:

- the field exists in a public upstream protocol artifact
- protobuf unknown-field behavior keeps omission safe for narrower peers
- local mapping and tests clearly document the behavior

Current examples:

- `submissionTime`
- `creationTime`
- `caveat`
- `releaseableTo`

These are treated as compatible public extensions, not as proof that every public client emits them.

## Policy For XML Detail

When XML `detail` content fits one of the typed protobuf messages exactly, `dpp.cot` may map it into that message.

When XML `detail` content does not fit exactly, `dpp.cot` should prefer preserving the full XML element in residual `xmlDetail`.

This includes cases such as:

- additional attributes on an otherwise known typed element
- repeated occurrences of an element intended for a single typed field
- nested structures that the typed protobuf model does not represent

This policy is derived from the public `detail.proto` rule set and reinforced by public ATAK CIV example payloads.

## Current Practical Policy

In practical terms, `dpp.cot` currently aims to be:

- conservative about protobuf conversion
- lossless about XML preservation
- explicit about public-source-backed extensions

If a future field or mapping rule is only visible in one public upstream implementation, it should be documented as a compatibility policy choice rather than a universal protocol truth unless a stronger normative source is available.
