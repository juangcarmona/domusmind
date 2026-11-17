# Design

<!-- pdac-scope: none reason="Documentation-only design; accepted product sources and their impact are linked in the accompanying map." -->

## Context

See the proposal. ProductShape owns intent, OpenSpec owns behavior specifications and accepted ADRs govern architecture. The owner has authorized deferring missing Assets execution evidence for this documentation-only slice.

## Goals / Non-Goals

**Goals:** Provide source-linked impact, architecture rationale, model gaps and an actionable verification inventory.

**Non-Goals:** Introduce runtime behavior, select an upload solution, create product rules or claim future acceptance.

## Decisions

- Use a durable documentation map with direct model and existing spec references rather than invent a new behavior contract. A zero-delta plan uses `skip_specs: true`.
- Record proposed implementation boundaries consistent with accepted ADRs, without changing their status or rewriting historical records. Alternatives and remaining decisions are explicit.
- Record missing lifecycle, behavior and executable coverage as gaps, not as evidence that a lifecycle is inapplicable. Route missing semantics through a Product Change before dependent implementation.

## Risks / Trade-offs

- [Prose mistaken for implemented functionality] → distinguish accepted intent, present evidence and future slice ownership.
- [Unresolved semantics filled by engineering assumptions] → identify product authority and block only dependent behavior.

## Migration Plan

Documentation only; no deployment or migration. Revert the new documentation and plan if withdrawn, without changing the accepted model.
