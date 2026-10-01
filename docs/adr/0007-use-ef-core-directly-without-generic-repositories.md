---
name: use-ef-core-directly-without-generic-repositories
description: Use EF Core as the persistence technology directly from slices, with no generic repository layer, loading aggregates explicitly for writes and projecting with AsNoTracking for reads.
status: Accepted
date: 2026-03-10
deciders: [Juan G. Carmona]
tags: [backend, persistence]
---

# ADR-0007: Use EF Core directly without generic repositories

## Context

This record is written retroactively. The legacy data model, the API conventions and `CLAUDE.md` state the rule as mandatory, but no decision record existed for it.

EF Core is the primary persistence technology in V1. Writes are aggregate-oriented: each command loads and modifies one aggregate. Reads are projection-oriented and serve stateless API requests, often composing data for UI surfaces. A common pattern wraps the ORM in generic repositories; that adds a layer between slices and the queries they need and hides EF Core's projection and tracking controls. The domain itself must not depend on EF Core.

## Decision

We will use EF Core directly from application slices through the DbContext, without a generic repository layer.

Write handlers load the aggregate they modify explicitly and persist it. Read queries use `AsNoTracking()` and express projections directly in LINQ, materialising straight into API models or read models. Access stays module-aware: each slice queries the data its module owns and references other aggregates by ID. EF Core stays out of the domain.

## Consequences

- **Positive:** Slices express exactly the query they need, and reads avoid change-tracking overhead. No abstraction layer to maintain or to leak through.
- **Negative:** The application layer depends on EF Core. Module-aware access relies on discipline, because every slice can reach every entity set through the shared context.
- **Neutral:** Checked against the code on 2026-10-01, the rule holds: slices use `IDomusMindDbContext` (a thin application-side interface exposing EF Core `DbSet`s, implemented by `DomusMindDbContext`), query handlers use `AsNoTracking()` extensively, and there is no generic repository. Two narrow, non-generic repository-style classes exist in infrastructure for authentication users and system initialisation, behind application interfaces. The Domain project has no EF Core reference.

## Alternatives considered

- **Generic repository layer over EF Core**: rejected because the data model states no generic repository layer is required and slices may query the DbContext directly.
- **Reading through tracked aggregates and mapping afterwards**: ruled out by the rule itself, which prefers direct projection with `AsNoTracking()` for efficient reads.

## References

- Legacy sources: `docs/07_platform/data-model.md` (EF Core Usage, Read Model), `docs/06_interfaces/api.md` (Queries)
- Rule: [`CLAUDE.md`](../../CLAUDE.md) (mandatory architectural rules)
- Evidence: [`src/backend/DomusMind.Application/Abstractions/Persistence/IDomusMindDbContext.cs`](../../src/backend/DomusMind.Application/Abstractions/Persistence/IDomusMindDbContext.cs), [`src/backend/DomusMind.Infrastructure/Persistence/DomusMindDbContext.cs`](../../src/backend/DomusMind.Infrastructure/Persistence/DomusMindDbContext.cs), [`src/backend/DomusMind.Application/Features/`](../../src/backend/DomusMind.Application/Features/)
- Related: [ADR-0004](0004-build-a-domain-centric-modular-monolith.md), [ADR-0006](0006-map-explicitly-without-automapper.md)
