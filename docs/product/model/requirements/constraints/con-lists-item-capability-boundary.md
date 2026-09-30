---
id: CON-LISTS-ITEM-CAPABILITY-BOUNDARY
type: constraint
title: "List items carry no assignment, status lifecycle or task-like extras"
status: draft
applies-to:
  - BC-LISTS
uses-terms:
  - TERM-LIST-ITEM
  - TERM-TASK
provenance:
  source: "docs/_legacy/04_contexts/shared-lists-item-model.md (Invariants 8-9, What This Model Does NOT Include); docs/_legacy/00_product/surfaces/lists.md (What items do not include, Anti-Patterns: Model drift); interview: product owner decision Q-0028 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Constraint

A list item has no assignee and no status beyond checked/unchecked, and these must never be introduced (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, Invariants 8-9). Steps or subtasks, attachments, comments, labels or tags, estimated effort and links to tasks are deliberately excluded (deferred) (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, What This Model Does NOT Include; docs/_legacy/00_product/surfaces/lists.md, What items do not include).

An item-level target person ("who it is for"), an item-level Area and a list colour are also outside the baseline; the item capability boundary holds (decided: Q-0028). The current code carries an optional item Area and target person on items and a colour on lists (observed: src/backend/DomusMind.Domain/Lists/ListItem.cs, ItemAreaId, TargetMemberId; src/backend/DomusMind.Domain/Lists/SharedList.cs, Color); that is implementation drift, not product capability.

## Rationale

Introducing these without a clear product requirement would collapse the boundary between Lists and Tasks (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, What This Model Does NOT Include).

## Consequences

Anchoring work to a person requires a Task. Any future deferred capability needs an explicit product change that revisits this boundary.
