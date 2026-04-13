# Spec - Get Family Lists

## Purpose

Retrieve all active lists belonging to a household.

---

## Context

- Module: Lists
- Read Model: `ListSummary`
- Slice: `get-family-lists`
- Query: `GetFamilyLists`

---

## Inputs

Required:

- `familyId`

---

## Preconditions

- family must exist

---

## Query Behavior

- retrieve all active (non-archived) lists for the given family
- return as summaries, not full item detail

---

## Result

Each list summary includes:

Required:

- `id`
- `name`
- `uncheckedCount`

Optional (when set on the list):

- `areaId`
- `linkedPlanId`
- `kind`

---

## Failure

- family not found

---

## Constraints

- This query drives the list switcher panel in the Lists surface. It must be optimized for fast loading and frequent access.
- Summaries must not include item-level detail or item content.
- No task-like metadata is returned. There are no due dates, assignees, priorities, or status values on lists or their summaries.
- `uncheckedCount` is the primary relevance signal. `itemCount` is not required at this level.
- Archived lists must not appear unless explicitly requested.