# Sources

This page tracks the public sources currently used in the docs for this repo.

## Local Copies Of Public Sources

- TAK protocol text: [`dpp.cot/protobuf/protocol.txt`](../dpp.cot/protobuf/protocol.txt)
- TAK protobuf definitions in [`dpp.cot/protobuf/`](../dpp.cot/protobuf/)
- CoT type data: [`dpp.cot/CoTtypes.xml`](../dpp.cot/CoTtypes.xml)

Validation status for these files is tracked in [Source Validation](source-validation.md).

Current summary:

- the protobuf family and `protocol.txt` are exact public ATAK CIV upstream matches
- `CoTtypes.xml` has validated public prior art and is semantically equivalent to public MITRE-derived mirrors, with one obvious upstream typo corrected locally

## External Public Sources

- MITRE CoT overview PDF: <https://www.mitre.org/sites/default/files/pdf/09_4937.pdf>
- ATAK CIV client repository: <https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV>
- TAK Product Center Server repository: <https://github.com/TAK-Product-Center/Server>
- JMS-2525 repository: <https://github.com/mil-standards/JMS-2525>
- PyTAK repository: <https://github.com/snstac/pytak>
- taky repository: <https://github.com/tkuester/taky>
- FreeTakServer repository: <https://github.com/FreeTAKTeam/FreeTakServer>

## Public Implementation Reference

These repositories are useful as public implementation references for interoperability context, surrounding protocol behavior, and public fixture sourcing.

Primary public implementation references:

- `deptofdefense/AndroidTacticalAssaultKit-CIV` for the public ATAK client implementation
- `TAK-Product-Center/Server` for the public TAK server implementation

It should be treated as:

- a public implementation source
- useful for observing practical client and server behavior and surrounding documentation
- not a substitute for normative protocol artifacts such as the committed `.proto` files and protocol text in this repo

Public fixture notes:

- `tkuester/taky` includes CoT payloads in public tests that are useful for round-trip and residual-detail coverage
- `snstac/pytak` includes public tests for generated CoT event structure and common default fields
- `FreeTAKTeam/FreeTakServer` includes public implementation examples and embedded CoT samples that are useful for interoperability context

## Repo-Derived Support

These are implementation support sources, not normative specifications:

- [`dpp.cot.Tests/SerializationTests.cs`](../dpp.cot.Tests/SerializationTests.cs)
- [`dpp.cot.Tests/MessageTests.cs`](../dpp.cot.Tests/MessageTests.cs)
- [`dpp.cot.Tests/ProtoMappingTests.cs`](../dpp.cot.Tests/ProtoMappingTests.cs)
- [`dpp.cot.Tests/StreamingTests.cs`](../dpp.cot.Tests/StreamingTests.cs)
