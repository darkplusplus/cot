# dpp.cot [![Status](https://github.com/darkplusplus/cot/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/darkplusplus/cot/actions/workflows/ci.yml)

A library for handling Cursor-on-Target messages.

## Package

NuGet package: `dpp.cot`

```bash
dotnet add package dpp.cot
```

## Documentation

This repo includes a MkDocs scaffold in [`mkdocs.yml`](mkdocs.yml) and [`docs/`](docs/index.md).

The docs are organized around:

- supported protocol behavior
- public source attribution
- implementation notes backed by code and tests

## Current Coverage

The library currently covers:

- CoT XML `event` parsing and serialization
- TAK protobuf payload v1 mapping
- TAK streaming envelope framing

## Target Frameworks

- `netstandard2.0`
- `net10.0`

## Release Notes

See [CHANGELOG.md](CHANGELOG.md).

## Source Policy

Public documentation in this repo should stay grounded in:

- public protocol artifacts committed in the repo
- public external sources explicitly linked in the docs
- implementation behavior demonstrated by tests

See:

- [docs/scope.md](docs/scope.md)
- [docs/source-policy.md](docs/source-policy.md)
- [docs/sources.md](docs/sources.md)
