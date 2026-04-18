# Changelog

## 2.0.0

- separate the XML/domain model from protobuf wire DTOs and explicit mapping
- add TAK streaming envelope support and stronger framed message parsing
- preserve unknown and mixed `detail` XML more faithfully, including typed elements with extra attributes
- add public-source-backed fixture coverage from ATAK CIV and `taky`
- document source inventory, fixture provenance, compatibility policy, and type inference
- multi-target the library for `netstandard2.0` and `net10.0`
