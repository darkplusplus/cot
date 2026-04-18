# Source Validation

This page records how local protocol artifacts relate to public source material.

## Exact Public Upstream Matches

The following local files were validated as exact Git blob matches against public files in `deptofdefense/AndroidTacticalAssaultKit-CIV`.

Local path -> public upstream

- [`dpp.cot/protobuf/contact.proto`](../dpp.cot/protobuf/contact.proto) -> <https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV/blob/main/takproto/contact.proto>
- [`dpp.cot/protobuf/cotevent.proto`](../dpp.cot/protobuf/cotevent.proto) -> <https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV/blob/main/takproto/cotevent.proto>
- [`dpp.cot/protobuf/detail.proto`](../dpp.cot/protobuf/detail.proto) -> <https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV/blob/main/takproto/detail.proto>
- [`dpp.cot/protobuf/group.proto`](../dpp.cot/protobuf/group.proto) -> <https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV/blob/main/takproto/group.proto>
- [`dpp.cot/protobuf/precisionlocation.proto`](../dpp.cot/protobuf/precisionlocation.proto) -> <https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV/blob/main/takproto/precisionlocation.proto>
- [`dpp.cot/protobuf/status.proto`](../dpp.cot/protobuf/status.proto) -> <https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV/blob/main/takproto/status.proto>
- [`dpp.cot/protobuf/takcontrol.proto`](../dpp.cot/protobuf/takcontrol.proto) -> <https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV/blob/main/takproto/takcontrol.proto>
- [`dpp.cot/protobuf/takmessage.proto`](../dpp.cot/protobuf/takmessage.proto) -> <https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV/blob/main/takproto/takmessage.proto>
- [`dpp.cot/protobuf/takv.proto`](../dpp.cot/protobuf/takv.proto) -> <https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV/blob/main/takproto/takv.proto>
- [`dpp.cot/protobuf/track.proto`](../dpp.cot/protobuf/track.proto) -> <https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV/blob/main/takproto/track.proto>
- [`dpp.cot/protobuf/protocol.txt`](../dpp.cot/protobuf/protocol.txt) -> <https://github.com/deptofdefense/AndroidTacticalAssaultKit-CIV/blob/main/commoncommo/core/impl/protobuf/protocol.txt>

These files are kept here as exact local copies of the corresponding public ATAK CIV artifacts.

## Public Prior Art With Validated Equivalent Data

- [`dpp.cot/CoTtypes.xml`](../dpp.cot/CoTtypes.xml)

Public source material:

- the local file header identifies it as MITRE CoT type material
- public mirrors of MITRE-derived `CoTtypes.xml` exist, including:
  - <https://github.com/dB-SPL/cot-types/blob/main/CoTtypes.xml>
  - <https://gist.github.com/marshalltesseract/e9ec8098427a5796d3c7885afb255554>

Validation notes:

- public antecedent is clear
- exact byte-for-byte equality was not found
- structural validation shows the local file is semantically identical to the public mirrors after correcting one obvious public typo: `zot="a-.-A-C"` should be `cot="a-.-A-C"`

This file is best understood as a corrected MITRE-derived public copy with equivalent data to the public mirrors listed above.

## Documentation Rule

When noting sources, this repo distinguishes between:

- exact public upstream copy
- public prior art / validated equivalent data
- local implementation artifact
