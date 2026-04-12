# Spec - Create List

## Purpose

Create a new persistent household list.

---

## Context

- Module: Lists
- Aggregate: `SharedList`
- Slice: `create-shared-list`
- Command: `CreateSharedList`

---

## Inputs

Required:

- `familyId`
- `name`

Optional:

- `areaId` — associates the list with a household area as contextual memory
- `linkedPlanId` — associates the list with a plan as a reference; the list remains a list regardless of this link
- `kind` — system-level classification; not required for creation or immediate use

---

## Preconditions

- family must exist
- name must be non-empty

---

## Behavior

- create a new `SharedList` aggregate for the given family
- initialize with an empty item collection
- apply optional area association and plan linkage if provided
- do not block creation on missing optional fields

---

## Result

- `listId`
- `name`

---

## Failure

- family not found
- name is empty or invalid

---

## Constraints

- A list can be created with a name alone. All other fields are optional and secondary.
- `kind`, `areaId`, and `linkedPlanId` may be set at creation or updated after creation. They do not affect the list's core identity or behavior.
- Linking to a plan does not alter list semantics. A linked list is still a list. Its items do not become scheduled work or tasks.
- List items must not be initialized with task-like fields. No due dates, reminders, assignees, or priority values belong to this aggregate.
