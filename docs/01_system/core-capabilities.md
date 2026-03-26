# DomusMind — Core Capabilities

## Purpose

This document defines the minimal set of capabilities required for the current core version of DomusMind.

These capabilities establish the core household operating model.

They map directly to **vertical slices and system specifications**.

---

# Capability Principles

Core capabilities must:

- operate on domain state
- reflect real household operations
- remain small and composable
- map directly to domain aggregates

Capabilities should represent **meaningful domain actions**, not technical operations.

---

# Core Capabilities (V1)

## Family Management

Create and manage the household structure.

Capabilities:

```
Create Family
Add Member
```

---

## Responsibility Management

Define ownership of household domains.

Capabilities:

```

Create Responsibility Domain
Assign Primary Owner
Assign Secondary Owners
Transfer Responsibility

```

---

## Event Scheduling

Manage the family timeline.

Capabilities:

```
Schedule Event
Reschedule Event
Cancel Event
Add Event Participants
Remove Event Participants
Add Reminder
Remove Reminder
View Family Timeline
```

---

## Task and Routine Management

Track actions required by household state.

Capabilities:

```
Create Task
Assign Task
Reassign Task
Complete Task
Cancel Task
Reschedule Task
Create Routine
Update Routine
Pause Routine
Resume Routine
```

---

## Shared Lists

Provide persistent household list-based coordination.

Capabilities:

```
Create Shared List
Rename Shared List
Delete Shared List
Add Item To Shared List
Update Shared List Item
Remove Shared List Item
Toggle Shared List Item
Reorder Shared List Items
Get Family Shared Lists
Get Shared List Detail
```

---

# Capability Scope

Core capabilities must remain **small and independent**.

Each capability should map to:

- a domain command
- a vertical slice
- a system specification

Example:

```
Schedule Event
→ command
→ schedule-event slice
→ specs/features/calendar/schedule-event.md
```

This structure ensures traceability between:

```

domain model
architecture
implementation
product capabilities

```