---
name: build-a-domain-centric-modular-monolith
description: Build DomusMind as a single deployable, domain-centric, API-first modular monolith whose bounded contexts are modules implemented as vertical slices across separate layer projects.
status: Accepted
date: 2026-03-10
deciders: [Juan G. Carmona]
tags: [architecture, backend, deployment]
---

# ADR-0004: Build a domain-centric modular monolith

## Context

This record is written retroactively. The legacy architecture documents and `CLAUDE.md` state the rule as mandatory, but no decision record existed for it.

DomusMind models and operates a household as a structured system. The legacy architecture puts the domain model at the stable centre: technology, persistence and user interfaces are secondary, and the domain must never depend on API contracts, persistence models, messaging formats, infrastructure or AI services. All interaction goes through one capability-oriented API shared by every client. The system is divided into bounded contexts that own their models and invariants, and capabilities are delivered as vertical slices rather than technical layers. Self-hosted families need a system that is simple to install and operate.

## Decision

We will build DomusMind as a domain-centric, API-first modular monolith: one deployable backend in which bounded contexts are modules, capabilities are vertical slices inside each module, and the domain model is the framework-agnostic centre.

The backend keeps separate projects per layer (Domain, Application, Contracts, Infrastructure, Api) and does not collapse into a single project; modules are expressed as bounded-context folders within them (for example `Features/<Context>/<Slice>`). The Domain project has no framework or infrastructure dependencies. Modules collaborate through domain events and explicit contracts rather than direct coupling. A single deployable keeps self-hosting simple while module and layer boundaries preserve independent feature evolution.

## Consequences

- **Positive:** One unit to build, deploy and operate, which suits self-hosting. Bounded contexts and slices can evolve independently inside that unit. The domain stays testable and free of infrastructure concerns.
- **Negative:** Module boundaries are enforced by convention and project references, not by process isolation, so they can erode without review. All modules scale and fail together.
- **Neutral:** Checked against the code on 2026-10-01, the rule holds: the solution has the five layer projects plus an Aspire AppHost, `DomusMind.Domain` has no package references, and the release is one container (API serving the built web app) next to PostgreSQL. All modules share one `DomusMindDbContext` and database, so storage ownership per module is a convention. `DomusMind.Contracts` references `DomusMind.Domain` although no contract uses a domain type. Cross-module collaboration through events is not yet realised (see [ADR-0008](0008-collaborate-across-modules-through-persisted-domain-events.md)).

## Alternatives considered

- **Distributed system of separately deployed services**: rejected because the legacy container view states V1 is a modular monolith, not a distributed system, and the identity decision already judged distributed complexity to bring no V1 benefit.
- **A single backend project**: ruled out by the rule itself, which forbids collapsing the modular monolith into one project and losing its layer boundaries.
- **Organising code by technical layer instead of capability**: rejected because slices must represent domain capabilities, not technical layers, and generic Clean Architecture boilerplate that conflicts with the documented design is ruled out.

## References

- Legacy sources: `docs/02_architecture/architecture.md`, `docs/02_architecture/application-model.md`, `docs/02_architecture/c4/02-containers.md`
- Rule: [`CLAUDE.md`](../../CLAUDE.md) (mandatory architectural rules)
- Evidence: [`DomusMind.slnx`](../../DomusMind.slnx), [`src/backend/DomusMind.Domain/DomusMind.Domain.csproj`](../../src/backend/DomusMind.Domain/DomusMind.Domain.csproj), [`src/backend/DomusMind.Application/Features/`](../../src/backend/DomusMind.Application/Features/), [`src/backend/DomusMind.Api/Dockerfile`](../../src/backend/DomusMind.Api/Dockerfile), [`deploy/docker-compose.yml`](../../deploy/docker-compose.yml)
- Related: [ADR-0001](0001-use-an-internal-application-mediator.md), [ADR-0002](0002-keep-authentication-local-and-separate-from-member-identity.md), [ADR-0008](0008-collaborate-across-modules-through-persisted-domain-events.md)
