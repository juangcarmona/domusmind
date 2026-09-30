---
id: BC-LISTS
type: bounded-context
title: "Lists"
status: draft
provenance:
  source: "openspec/specs/lists/spec.md (Purpose); docs/_legacy/04_contexts/shared-lists.md (Purpose, Responsibilities, Boundaries With Other Contexts); docs/_legacy/00_product/surfaces/lists.md (Conceptual Model, Household Boundaries); corroborated by src/backend/DomusMind.Domain/Lists/**; interview: product owner decision Q-0026 (E-0158); interview: product owner decision Q-0027 (E-0158); interview: product owner decision Q-0028 (E-0158); interview: product owner decision Q-0029 (E-0158); interview: product owner decision Q-0031 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Responsibility

Lists is the household's reusable execution container for captured items: what should be remembered, bought, checked, prepared or done next time (observed: openspec/specs/lists/spec.md, Purpose). It covers creating, updating, archiving, restoring and permanently deleting lists (deletion decided: Q-0026); adding, updating, checking, removing and ordering list items; marking items as important; and giving items optional temporal fields (due date, reminder, repeat) that make them appear in the Agenda (observed: docs/_legacy/04_contexts/shared-lists.md, Responsibilities).

List state is shared across the household in real time (observed: openspec/specs/lists/spec.md, Purpose). Lists are shared with the household by default and can optionally be private to one person (decided: Q-0027). Lists are reusable by design: items are consumed within a use, the list persists across uses (observed: openspec/specs/lists/spec.md, Purpose; docs/_legacy/00_product/surfaces/lists.md, Lifecycle). An archived list is read-only until restored (decided: Q-0031). A list's kind is optional and open-ended, with a generic default when none is chosen (decided: Q-0029).

## Language

Household-facing language speaks of a **List** and its **List Items**; legacy domain documents call them Shared List / SharedList and the aggregate is still named SharedList in the code (observed: openspec/specs/lists/spec.md, Notes: Terminology). The language is toggle-based and capability-oriented: an item always has a name and a checked state, and may progressively carry quantity, note, importance and temporal fields (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, Capability Groups). There is no completion lifecycle and no assignment vocabulary here (observed: docs/_legacy/04_contexts/shared-lists.md, Design Notes; docs/_legacy/04_contexts/shared-lists-item-model.md, Invariants 8-9).

## Boundaries

- A list is not a task board and a list item is not a task; items never become tasks automatically (observed: openspec/specs/lists/spec.md, Purpose; docs/_legacy/04_contexts/shared-lists.md, SharedListItem).
- Lists do not own time: they reference it through item temporal fields, and the Agenda projects those items read-only. A list item's repeat is a lightweight recurrence hint, not a plan's recurrence (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, Key ownership boundary).
- Lists do not own accountability: an Area association is contextual and does not affect ownership (observed: openspec/specs/lists/spec.md, Purpose; docs/_legacy/00_product/surfaces/lists.md, Relationship with Areas).
- Assignment to a person and status lifecycles beyond checked/unchecked are outside this context (observed: docs/_legacy/04_contexts/shared-lists-item-model.md, What This Model Does NOT Include). Item target person, item Area and list colour are also outside the baseline (decided: Q-0028).

## External Relationships

- **Household (BC-FAMILY)**: every list belongs to exactly one household; Lists depends on the household but never modifies it (observed: docs/_legacy/04_contexts/shared-lists.md, Boundaries: Family).
- **Areas (BC-RESPONSIBILITIES)**: a list may be associated with an Area as contextual memory for that Area; it is navigable from the Area but behaves like any other list (observed: docs/_legacy/00_product/surfaces/lists.md, Relationship with Areas).
- **Plans and Agenda (BC-CALENDAR)**: a list may be linked to a Plan; the plan shows a compact cue with the list name and unchecked count. Items with temporal fields appear in the Agenda as a distinct, read-only entry type that is edited only through Lists (observed: openspec/specs/lists/spec.md, Agenda Projection; docs/_legacy/00_product/surfaces/lists.md, Relationship with Agenda).
- **Tasks (BC-TASKS)**: distinct models; no operation crosses from one into the other (observed: docs/_legacy/04_contexts/shared-lists.md, Boundaries: Tasks).
- **Meal Planning (BC-MEAL-PLANNING)**: shopping lists generated from meal plans arrive as a list of kind shopping and are then treated as regular lists; how they arrive is owned by Meal Planning (observed: openspec/specs/lists/spec.md, Notes: Shopping list creation; docs/_legacy/04_contexts/shared-lists.md, Domain Events reacted to).
