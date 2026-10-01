---
id: UC-ROADMAP-SEE-RENEWALS-AND-DEADLINES
type: use-case
title: "See upcoming renewals and deadlines"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-ADMINISTRATION"
governed-by: []
uses-terms:
  - "TERM-HOUSEHOLD"
  - "TERM-AGENDA"
  - "TERM-RENEWAL-DEADLINE"
provenance:
  source: "docs/_legacy/09_roadmap/roadmap.md (V2 - Household Domain Expansion); docs/_legacy/01_system/system-overview.md (System Boundaries); docs/_legacy/01_system/system-spec.md (Out of Scope for V1); docs/_legacy/03_domain/ubiquitous-language.md (Terms to Avoid); decision Q-0004"
  confidence: "low"
  recovered-from: "inference"
---

## Goal

Planned: V2. The household sees upcoming renewals and administrative deadlines early enough to act (observed: roadmap.md, V2 "renewal and deadline visibility").

## Trigger

The household wants this part of home life to stop depending on one person's memory (inferred from strategy.md, Mission).

## Preconditions

The household exists.

## Main Flow

1. A person records an obligation with its deadline (inferred).
2. DomusMind shows it ahead of the deadline where the household looks at time (inferred).

## Alternative Flows

Not described in the sources.

## Failure Conditions

Not described in the sources.

## Postconditions

The obligation is visible to the household before it is due (inferred).

Only the capability name comes from the roadmap; the flow is an inferred sketch for review.
