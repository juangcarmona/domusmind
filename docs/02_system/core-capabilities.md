# DomusMind — Core Capabilities

## Purpose

This document defines the **minimal set of capabilities** required for the first functional version of DomusMind.

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
Remove Member
Define Relationships
Add Dependent
Add Pet

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
Create Reminder

```

---

## Tasks

Track actions required by household state.

Capabilities:

```

Create Task
Assign Task
Complete Task
Generate Task From Event

```

---

## Routines

Define recurring operational patterns.

Capabilities:

```

Create Routine
Update Routine
Trigger Routine
Generate Tasks From Routine

```

---

## Timeline

Provide visibility into household activity.

Capabilities:

```

View Family Timeline
View Member Timeline
View Upcoming Events

```

---

# Supporting Capabilities

These capabilities enable core functionality but may evolve later.

Examples:

```

Register Property
Store Document
Track Inventory Item
Create Meal Plan
Generate Shopping List

```

---

# Capability Evolution

DomusMind capabilities evolve in layers:

```

Structure
→ Coordination
→ Anticipation
→ Automation
→ Intelligent Assistance

```

Initial capabilities focus on **structure and coordination**.

Future capabilities may introduce automation and AI-driven suggestions.

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
→ specs/features/schedule-event.md

```

This structure ensures traceability between:

```

domain model
architecture
implementation
product capabilities

```