---
id: UC-ROADMAP-TRACK-IMPORTANT-DOCUMENTS
type: use-case
title: "Keep track of important household documents"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by: []
uses-terms:
  - "TERM-HOUSEHOLD"
  - "TERM-MEMBER"
  - "TERM-IMPORTANT-DOCUMENT"
provenance:
  source: "docs/_legacy/09_roadmap/roadmap.md (V2 - Household Domain Expansion); docs/_legacy/01_system/system-overview.md (System Boundaries); docs/_legacy/01_system/system-spec.md (Out of Scope for V1); docs/_legacy/03_domain/ubiquitous-language.md (Terms to Avoid); decision Q-0004"
  confidence: "low"
  recovered-from: "inference"
---

## Goal

Planned: V2. The household knows which important documents it has, for whom, and when they expire (observed: roadmap.md, V2 "important document tracking"; expiry is inferred).

## Trigger

The household wants this part of home life to stop depending on one person's memory (inferred from strategy.md, Mission).

## Preconditions

The household exists.

## Main Flow

1. A person records a document and the person it belongs to (inferred).
2. The household can find it later (inferred).

## Alternative Flows

Not described in the sources.

## Failure Conditions

Not described in the sources.

## Postconditions

The document is recorded for the household (inferred).

Only the capability name comes from the roadmap; the flow is an inferred sketch for review.
