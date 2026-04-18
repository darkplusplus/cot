# Source Policy

This repository should keep its public documentation grounded in public sources and repo-local implementation work.

## Allowed Source Types

- public protocol text committed in this repo
- public `.proto` files committed in this repo
- public XML examples used in tests
- public external documents explicitly linked in [Sources](sources.md)
- public implementation repositories explicitly linked in [Sources](sources.md)
- implementation behavior demonstrated by code and tests in this repo

## Documentation Rules

- cite a public source for normative protocol claims
- label implementation-repository evidence as implementation context, not as the normative spec
- label repo-specific behavior as an `Implementation Note`
- keep repo-derived evidence separate from external protocol sources

## Exclusions

This documentation set should not rely on non-public or access-controlled material.

If a behavior cannot be supported from public sources, the public docs should either:

1. omit the claim, or
2. document only the observed implementation behavior as an implementation note
