<!-- pdac-scope: cited -->

# Proposal

## Why

Households need durable identities for things they manage, without confusing where a thing is with what contains it or who is responsible. The accepted asset lifecycle requires safe archive and restoration without losing information or history.

<!-- pdac:cite id="FR-ASSETS-REGISTER-AND-ORGANIZE" digest="sha256:6fb7c5fa2cf4f2523b5fd0b829a5b94d6ec3aa35367f8c9aca8adba9bf917ab8" -->

## What Changes

- Introduce household-scoped assets and separately identifiable reusable locations.
- Support common details, category, optional containment and independent location, member and Area references.
- Enforce household authorization, scoped references and acyclic containment.
- Archive ordinary removals and allow authorized read-only consultation and explicit restoration without cascading changes.

## Capabilities

### New Capabilities

- `household-assets`: household-scoped asset identity, organization and lifecycle.

### Modified Capabilities

None.

## Impact

Adds backend domain, application, API contracts, persistence and tests. No interface or product-model changes are included; definitive deletion and history creation are outside this slice.
