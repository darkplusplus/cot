# Type Inference

This page describes how `dpp.cot` interprets CoT type strings and other compact taxonomy fields.

## Source Basis

The type and predicate material in this repo is grounded in public source material:

- [`dpp.cot/CoTtypes.xml`](../dpp.cot/CoTtypes.xml)
- the MITRE CoT overview linked in [Sources](sources.md)
- public ATAK CIV example payloads under `takcot/examples/`
- the public JMS-2525 repository: <https://github.com/mil-standards/JMS-2525>

The `CoTtypes.xml` header explicitly states that upper-case portions of the CoT type hierarchy are taken from the MIL-STD-2525 type hierarchy, while lower-case characters are CoT extensions.

## What The Library Infers

`dpp.cot` currently uses CoT type data in three practical ways:

- descriptions for known type strings
- regex-style predicates over type strings
- grouping logic for common families such as air, ground, route, point, weather, and tasking

These behaviors are exposed through:

- [`CotDescriptions.cs`](../dpp.cot/CotDescriptions.cs)
- [`CotPredicates.cs`](../dpp.cot/CotPredicates.cs)

The current generated tables in those files are derived from `CoTtypes.xml`.

## What The Library Does Not Claim

`dpp.cot` does not claim that:

- every CoT type string is fully specified by a single public source
- CoT type parsing is equivalent to implementing the full MIL-STD-2525 symbology standard
- the library can render or validate complete 2525 symbol semantics
- every lower-case extension seen in CoT is part of MIL-STD-2525 itself

In practice, the library treats the CoT type string as a stable taxonomy key and uses public CoT type data to attach descriptions and predicates where available.

## Inference Categories

For this repo, type-related statements generally fall into one of these categories:

### Direct Type Mapping

Statements that come directly from `CoTtypes.xml`, such as:

- a known type string description
- a known `is` predicate
- a known `how` mapping

### Public-Lineage Inference

Statements that rely on the public lineage between CoT type data and MIL-STD-2525/JMS, such as:

- upper-case path segments correspond to the 2525 military symbology hierarchy
- lower-case segments often represent CoT-side extensions or refinements

### Implementation Inference

Statements about how `dpp.cot` groups or exposes type information in code, such as:

- convenience predicates
- helper methods
- generated lookup tables

These should be understood as repo behavior, even when they are built from public source material.

## Examples

Public ATAK CIV example payloads reinforce that real CoT products rely on these compact type keys:

- marker examples use values such as `a-u-G` and `b-m-p-s-m`
- route examples use `b-m-r`
- geofence examples use `u-d-c-c`

These payloads show that CoT type strings carry operational meaning even when the surrounding `detail` tree is highly product-specific.

## Documentation Rule

When documenting type semantics in this repo:

- cite `CoTtypes.xml` for direct CoT mapping data
- cite JMS-2525 or other public military symbology sources for lineage or taxonomy context
- label repo-specific grouping behavior as implementation behavior rather than universal protocol fact
