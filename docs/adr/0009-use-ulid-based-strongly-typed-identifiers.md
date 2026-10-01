---
name: use-ulid-based-strongly-typed-identifiers
description: Identify every aggregate with its own strongly typed ULID-based record struct, serialized as a string outside the domain; never implemented, superseded by ADR-0010 (GUID-backed identifiers).
status: Superseded by ADR-0010
date: 2026-10-01
deciders: [Juan G. Carmona]
tags: [backend, domain, persistence, api]
---

# ADR-0009: Use ULID-based strongly typed identifiers

## Context

This record is written retroactively from the rule stated in the legacy identifier strategy and data model; `CLAUDE.md` does not repeat it. The documented format never took effect: the implementation has used GUIDs from its first migration (see Consequences). It is kept for the record and superseded by [ADR-0010](0010-use-guid-backed-strongly-typed-identifiers.md), which records the identifier decision actually in effect.

Identifiers are part of the domain contract and must stay stable across versions, deployments and integrations. The legacy strategy requires them to be globally unique so that mobile clients, web clients, backend services and integrations can create entities without coordination; safe to generate on clients to support offline-first and optimistic UI; immutable and free of mutable information; and not interchangeable between aggregates.

## Decision

We will identify each aggregate with its own strongly typed identifier, implemented as a `readonly record struct` value object wrapping a ULID, and serialize identifiers as strings at the API and in storage.

ULID is chosen because it is globally unique, sortable by creation time, URL-safe, shorter than a UUID and widely supported across .NET, JavaScript/TypeScript and mobile platforms. Per-aggregate types give compile-time safety and explicit domain semantics. Storage uses a fixed-length string (`CHAR(26)` or equivalent) for predictable size and lexicographic ordering. Domain events carry their own identifier, independent of aggregate identifiers.

## Consequences

- **Positive:** Strong typing prevents mixing identifiers of different aggregates at compile time. Client-side generation needs no server round trip, and creation-time ordering comes with the identifier.
- **Negative:** String storage is larger than a native binary UUID column, and a ULID library is needed on every platform that generates identifiers.
- **Neutral:** Checked against the code on 2026-10-01, only the strong typing is implemented. Every aggregate identifier is a `readonly record struct` in the domain, but each wraps a `Guid` created with `Guid.NewGuid()` (random, not time-sortable); no ULID library is referenced; contracts type identifiers as `Guid`; routes constrain them with `:guid`; and PostgreSQL stores them as `uuid` through EF Core value conversions. Event identifiers are also `Guid`. On 2026-10-01 the architecture review chose to record the GUID format rather than migrate, in [ADR-0010](0010-use-guid-backed-strongly-typed-identifiers.md); the reasoning lives there.

## Alternatives considered

- **UUID/GUID identifiers**: rejected by the strategy in favour of ULID, which is sortable by creation time and shorter; this is nevertheless what the code uses today.
- **Primitive, untyped identifiers**: ruled out by the rule itself, which requires identifiers that are not interchangeable between aggregates.
- **Server-assigned identifiers (for example database sequences)**: ruled out by the rule itself, which requires identifiers that clients can generate without coordination before persistence.

## References

- Legacy sources: `docs/02_architecture/id-strategy.md`, `docs/07_platform/data-model.md` (Identifier Strategy)
- Evidence: [`src/backend/DomusMind.Domain/Family/FamilyId.cs`](../../src/backend/DomusMind.Domain/Family/FamilyId.cs), [`src/backend/DomusMind.Infrastructure/Persistence/Configurations/Family/FamilyConfiguration.cs`](../../src/backend/DomusMind.Infrastructure/Persistence/Configurations/Family/FamilyConfiguration.cs), [`src/backend/DomusMind.Contracts/Family/CreateFamilyResponse.cs`](../../src/backend/DomusMind.Contracts/Family/CreateFamilyResponse.cs), [`src/backend/DomusMind.Api/Controllers/FamiliesController.cs`](../../src/backend/DomusMind.Api/Controllers/FamiliesController.cs)
- Superseded by: [ADR-0010](0010-use-guid-backed-strongly-typed-identifiers.md)
- Related: [ADR-0007](0007-use-ef-core-directly-without-generic-repositories.md), [ADR-0008](0008-collaborate-across-modules-through-persisted-domain-events.md)
