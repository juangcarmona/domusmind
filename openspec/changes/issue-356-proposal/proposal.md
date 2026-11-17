# Proposal

<!-- pdac-scope: none reason="Documentation-only plan; product source references are collected in the impact map, with no behavioral contract changes." -->

## Why

The accepted Household Assets model spans several contexts, but its implementation and behavioral specifications have not yet been delivered. Dependent slices need a reviewable map of the accepted boundaries, missing coverage and decisions before implementing them.

## What Changes

- Document the ProductShape-to-OpenSpec impact, architectural boundaries and verification responsibilities for the accepted MVP.
- Identify lifecycle and structured-behaviour gaps and the product-authority route for missing semantics.
- Preserve the accepted product model and ADRs; do not implement Assets or decide deferred product rules.

## Capabilities

### New Capabilities

None. This is documentation, not a behavioral specification.

### Modified Capabilities

None. Future slices must propose their own specification changes.

## Impact

Adds an impact and architecture decision map under `docs/` and this OpenSpec plan. No runtime, API, database, dependency or product-model changes are included.
