# DomusMind — System Spec

## Purpose

This document defines the functional scope of DomusMind V1 at system level.

It links:

- bounded contexts
- feature specifications
- core capabilities

It is the entry point for executable product scope.

---

## V1 Scope

DomusMind V1 includes four core bounded contexts:

- Family
- Responsibilities
- Calendar
- Tasks

These contexts provide the minimum viable household operating model.

---

## Core Capabilities

V1 supports the following capability groups:

- family structure management
- responsibility ownership
- event scheduling
- task execution
- routine management
- unified family timeline

---

## Context Map

### Family
Owns household identity and structure.

### Responsibilities
Owns accountability and ownership of household domains.

### Calendar
Owns events, schedules, participants, and reminders.

### Tasks
Owns tasks, routines, assignment, and completion.

---

## Context Dependencies

- Responsibilities depends on Family
- Calendar depends on Family
- Tasks depends on Family
- Tasks may react to Calendar events
- Tasks may reference Responsibility domains

---

## V1 Feature Set

### Family
- create-family
- add-member
- assign-relationship
- remove-member

### Responsibilities
- create-responsibility-domain
- assign-primary-owner
- assign-secondary-owner
- transfer-responsibility

### Calendar
- schedule-event
- reschedule-event
- cancel-event
- add-event-participant
- remove-event-participant
- add-reminder
- remove-reminder
- view-family-timeline

### Tasks
- create-task
- assign-task
- complete-task
- cancel-task
- reschedule-task
- create-routine
- update-routine
- pause-routine
- resume-routine

---

## Out of Scope for V1

The following are explicitly outside V1:

- properties
- documents
- food and meal planning
- inventory
- pets as separate operational context
- finance
- AI automation
- external integrations

---

## Implementation Rule

Every feature spec must map to:

- one bounded context
- one aggregate
- one vertical slice

No feature may bypass aggregate boundaries.

---

## Success Criteria

DomusMind V1 is complete when:

- household identity can be created
- members can be managed
- responsibilities can be assigned
- events can be scheduled
- tasks can be executed
- routines can be maintained
- timeline can be queried