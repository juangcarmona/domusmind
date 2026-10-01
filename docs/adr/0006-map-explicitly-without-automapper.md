---
name: map-explicitly-without-automapper
description: Map between domain, read models and explicit API models with hand-written code close to each slice, never exposing domain entities and never using AutoMapper.
status: Accepted
date: 2026-03-10
deciders: [Juan G. Carmona]
tags: [backend, api, application]
---

# ADR-0006: Map explicitly without AutoMapper

## Context

This record is written retroactively. The legacy API conventions and `CLAUDE.md` state the rule as mandatory, but no decision record existed for it.

The API contract is a set of explicit request and response models, separate from the domain model. Every slice has to translate between them: request models into commands, and aggregates or query results into response models. Convention-based mapping libraries such as AutoMapper can do this automatically, but they hide the shape of the contract behind configuration.

## Decision

We will map explicitly, with hand-written code kept close to the slice, and we will not use AutoMapper.

API models are contracts, not domain entities; domain entities are never exposed through the API. Query projections are expressed directly in EF Core queries when possible, so that reads materialise straight into response or read models. Keeping mapping explicit keeps contracts visible and avoids hidden conventions.

## Consequences

- **Positive:** The contract a client sees is readable in the slice that produces it. Renaming a domain member cannot silently change the API. Projections into response models translate into efficient SQL.
- **Negative:** Mapping code is written and maintained by hand and repeats for similar shapes.
- **Neutral:** Checked against the code on 2026-10-01, the rule holds: no project references AutoMapper or any other mapping library, handlers construct `Contracts` response records directly (often inside LINQ `Select` projections), and no contract uses a domain type. Contracts live under `DomusMind.Contracts.*` rather than the `Model.*` namespace the legacy documents name.

## Alternatives considered

- **AutoMapper**: rejected because the API conventions exclude it: convention-based mapping hides contracts behind implicit rules.
- **Exposing domain entities directly as API models**: ruled out by the rule itself, which forbids the API contract from exposing domain entities.

## References

- Legacy source: `docs/06_interfaces/api.md` (Models, Mapping)
- Rule: [`AGENTS.md`](../../AGENTS.md) (mandatory architectural rules)
- Evidence: [`src/backend/DomusMind.Contracts/`](../../src/backend/DomusMind.Contracts/), [`src/backend/DomusMind.Application/Features/`](../../src/backend/DomusMind.Application/Features/), project files under [`src/backend/`](../../src/backend/)
- Related: [ADR-0005](0005-expose-a-rest-api-through-aspnet-core-controllers.md), [ADR-0007](0007-use-ef-core-directly-without-generic-repositories.md)
