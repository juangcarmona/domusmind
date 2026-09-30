---
id: TERM-LIST
type: domain-term
title: "List"
status: draft
defined-in: BC-LISTS
synonyms:
  - "Shared List"
  - "SharedList"
uses-terms:
  - TERM-LIST-ITEM
  - TERM-AREA
  - TERM-PLAN
  - TERM-TASK
provenance:
  source: "openspec/specs/lists/spec.md (Purpose, Notes: Terminology); docs/_legacy/04_contexts/shared-lists.md (Aggregate Roots: SharedList); docs/_legacy/00_product/surfaces/lists.md (What a List Is); src/backend/DomusMind.Domain/Lists/SharedList.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

A named, household-owned, reusable execution container for grouped items. It answers "what should be remembered, bought, checked, prepared, or done next time?" A list persists across uses while its items are consumed within each use (observed: openspec/specs/lists/spec.md, Purpose; docs/_legacy/00_product/surfaces/lists.md, What a List Is). A list has a name, an optional kind, an optional Area association and an optional link to a Plan, and an ordered collection of list items (observed: openspec/specs/lists/spec.md, List Creation; docs/_legacy/04_contexts/shared-lists.md, SharedList). A list is either active or archived (observed: openspec/specs/lists/spec.md, List Archive).

## Distinguish From

- **Task**: a task has a structured lifecycle, assignment and management; a list is toggle-based with no completion lifecycle (observed: docs/_legacy/04_contexts/shared-lists.md, Design Notes).
- **Area**: an Area owns accountability; a list associated with an Area is only contextual memory for it (observed: docs/_legacy/00_product/surfaces/lists.md, Relationship with Areas).
- **Plan**: linking a list to a plan does not turn its items into scheduled work (observed: openspec/specs/lists/spec.md, Purpose).

## Usage

Household-facing term is "List"; legacy docs and the domain code use "Shared List"/"SharedList" (observed: openspec/specs/lists/spec.md, Notes: Terminology). Examples: groceries, packing for a trip, preparation for an event, restocking essentials, school preparation items (observed: docs/_legacy/00_product/surfaces/lists.md, What a List Is).
