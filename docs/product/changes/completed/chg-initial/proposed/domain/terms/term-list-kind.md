---
id: TERM-LIST-KIND
type: domain-term
title: "List Kind"
status: draft
defined-in: BC-LISTS
synonyms:
  - "kind"
  - "SharedListKind"
uses-terms:
  - TERM-LIST
provenance:
  source: "openspec/specs/lists/spec.md (List Creation, List Update); docs/_legacy/04_contexts/shared-lists.md (SharedList, Domain Events reacted to); src/backend/DomusMind.Domain/Lists/ValueObjects/ListKind.cs; interview: product owner decision Q-0029 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

An optional, system-level classification of a list's intended usage, settable at creation and changeable later (observed: openspec/specs/lists/spec.md, List Creation). The kind is optional and open-ended, not a fixed set; a list created without a chosen kind gets a generic default kind (decided: Q-0029). The code uses "general" as that default (observed: src/backend/DomusMind.Application/Features/Lists/CreateList/CreateListCommandHandler.cs) and stores the kind as open text (observed: ListKind.cs). The known kind is **shopping**, used for lists generated from meal plans (observed: docs/_legacy/04_contexts/shared-lists.md, Domain Events reacted to; decided: Q-0029).

## Distinguish From

- **Area association**: the kind classifies usage; an Area association anchors the list to a household area of accountability (observed: openspec/specs/lists/spec.md, List Creation).

## Usage

A person may choose any kind or none; when none is chosen the list gets the generic default kind (decided: Q-0029).
