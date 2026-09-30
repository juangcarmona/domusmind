---
id: UC-ROADMAP-PLAN-PROPERTY-MAINTENANCE
type: use-case
title: "Plan the home's maintenance"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-PROPERTY"
governed-by: []
uses-terms:
  - "TERM-HOUSEHOLD"
  - "TERM-AREA"
  - "TERM-PROPERTY-MAINTENANCE"
provenance:
  source: "docs/_legacy/09_roadmap/roadmap.md (V2 - Household Domain Expansion); docs/_legacy/01_system/system-overview.md (System Boundaries); docs/_legacy/01_system/system-spec.md (Out of Scope for V1); docs/_legacy/03_domain/ubiquitous-language.md (Terms to Avoid); decision Q-0004"
  confidence: "low"
  recovered-from: "inference"
---

## Goal

Planned: V2. The household plans recurring and one-off upkeep of the home so it is not forgotten (observed: roadmap.md, V2 "property maintenance planning").

## Trigger

The household wants this part of home life to stop depending on one person's memory (inferred from strategy.md, Mission).

## Preconditions

The household exists.

## Main Flow

1. A person records a maintenance need for the home (inferred).
2. The household schedules or assigns it (inferred).

## Alternative Flows

Not described in the sources.

## Failure Conditions

Not described in the sources.

## Postconditions

The maintenance is planned and visible (inferred).

Only the capability name comes from the roadmap; the flow is an inferred sketch for review.
