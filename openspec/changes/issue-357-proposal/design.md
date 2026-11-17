<!-- pdac-scope: cited -->

# Design

## Context

See the proposal and the accepted LC-ASSET and BR-ASSET-HIERARCHY-MEANING. Household membership and Areas are owned by existing modules; the asset context must reference them rather than change them.

<!-- pdac:cite id="FR-ASSETS-REGISTER-AND-ORGANIZE" digest="sha256:6fb7c5fa2cf4f2523b5fd0b829a5b94d6ec3aa35367f8c9aca8adba9bf917ab8" -->

## Goals / Non-Goals

**Goals:** Preserve household isolation, distinct relationships, acyclic containment and non-destructive lifecycle transitions.

**Non-Goals:** UI, permanent deletion, obligations, history creation and changes to accepted product intent.

## Decisions

- Use separate asset and location aggregates, matching their independent identity; a shared location is not containment. Embedding locations in assets would prevent safe reuse.
- Authorize at the application boundary and validate references against the same household before mutation. Unscoped identifier lookups would risk cross-household disclosure.
- Validate parent ancestry transactionally, including concurrent changes; checking only the immediate parent cannot prevent indirect or concurrent cycles.
- Preserve stored data on archive and restore, and reject ordinary edits while archived. Deletion or cascading lifecycle changes would violate the accepted model.
- Follow the existing mediator, explicit contracts and EF Core persistence. Tests span domain invariants and application/API isolation.

## Risks / Trade-offs

- Concurrent reparenting can introduce cycles → serialize competing household hierarchy changes and test the race.
- Historical record creation is not in this slice → preserve existing and future associations without claiming history creation is implemented.

## Migration Plan

Introduce additive tables with scoped indexes and safe rollback only when the new data is no longer needed; rollback must not silently discard household data.
