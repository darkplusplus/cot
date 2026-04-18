# dpp.cot

`dpp.cot` is a library for handling Cursor-on-Target messages.

This documentation set focuses on:

- supported protocol behavior
- source attribution
- implementation notes backed by tests

The goal is to make the supported parts of the library easier to understand and verify.

## Coverage

This repo currently documents and tests:

- CoT XML `event` parsing and serialization
- TAK protobuf payload v1 mapping
- TAK streaming envelope framing
- selected typed `detail` elements
- preservation of unsupported or unknown `detail` elements as residual XML

## Reading Guide

- start with [Scope](scope.md) for what these docs are and are not claiming
- use [Protocols](protocols/cot-xml.md) for wire-format behavior
- use [Library Design](library-design.md) for implementation structure
- use [Testing](testing.md) for executable support boundaries
- use [Sources](sources.md) for public source material used in the docs
