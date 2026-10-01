# Architecture Decision Records

This directory holds DomusMind's Architecture Decision Records: one file per significant decision, carrying its context, the decision, its consequences and the alternatives rejected. Records are numbered sequentially and never renumbered; a reversed decision gets a new record that supersedes the old one. The ADRs are authoritative for decision rationale and status, and they are indexed for architecture readers from [`docs/architecture/09-architecture-decisions.md`](../architecture/09-architecture-decisions.md), which maps each decision in effect to the architecture it affects.

| Number | Title | Status | Date |
| --- | --- | --- | --- |
| 0001 | [Use an internal application mediator](0001-use-an-internal-application-mediator.md) | Accepted | 2026-03-10 |
| 0002 | [Keep authentication local and separate from member identity](0002-keep-authentication-local-and-separate-from-member-identity.md) | Accepted | 2026-03-10 |
| 0003 | [Use delegated Graph auth for Outlook calendar ingestion](0003-use-delegated-graph-auth-for-outlook-calendar-ingestion.md) | Accepted | 2026-04-07 |
| 0004 | [Build a domain-centric modular monolith](0004-build-a-domain-centric-modular-monolith.md) | Accepted | 2026-03-10 |
| 0005 | [Expose a REST API through ASP.NET Core controllers](0005-expose-a-rest-api-through-aspnet-core-controllers.md) | Accepted | 2026-03-10 |
| 0006 | [Map explicitly without AutoMapper](0006-map-explicitly-without-automapper.md) | Accepted | 2026-03-10 |
| 0007 | [Use EF Core directly without generic repositories](0007-use-ef-core-directly-without-generic-repositories.md) | Accepted | 2026-03-10 |
| 0008 | [Collaborate across modules through persisted domain events](0008-collaborate-across-modules-through-persisted-domain-events.md) | Accepted | 2026-03-10 |
| 0009 | [Use ULID-based strongly typed identifiers](0009-use-ulid-based-strongly-typed-identifiers.md) | Superseded by ADR-0010 | 2026-10-01 |
| 0010 | [Use GUID-backed strongly typed identifiers](0010-use-guid-backed-strongly-typed-identifiers.md) | Accepted | 2026-10-01 |
