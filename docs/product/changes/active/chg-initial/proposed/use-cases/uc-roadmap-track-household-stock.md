---
id: UC-ROADMAP-TRACK-HOUSEHOLD-STOCK
type: use-case
title: "Know what the household has in stock"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-INVENTORY"
governed-by: []
uses-terms:
  - "TERM-HOUSEHOLD"
  - "TERM-LIST"
  - "TERM-SUPPLY-STATE"
provenance:
  source: "docs/_legacy/09_roadmap/roadmap.md (V2 - Household Domain Expansion); docs/_legacy/01_system/system-overview.md (System Boundaries); docs/_legacy/01_system/system-spec.md (Out of Scope for V1); docs/_legacy/03_domain/ubiquitous-language.md (Terms to Avoid); decision Q-0004"
  confidence: "low"
  recovered-from: "inference"
---

## Goal

Planned: V2. The household knows the state of its pantry and supplies, so restocking follows reality rather than memory (observed: roadmap.md, V2 "richer household stock awareness", "pantry or supply-state modeling").

## Trigger

The household wants this part of home life to stop depending on one person's memory (inferred from strategy.md, Mission).

## Preconditions

The household exists.

## Main Flow

1. A person records what is in stock and what is running low (inferred).
2. The household sees what needs restocking (inferred).

## Alternative Flows

Not described in the sources.

## Failure Conditions

Not described in the sources.

## Postconditions

The household's supply state is visible (inferred).

Only the capability name comes from the roadmap; the flow is an inferred sketch for review.
