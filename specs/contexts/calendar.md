# Context Spec — Calendar

## Purpose

Defines the functional scope of the Calendar context.

This context owns the temporal structure of household life.

---

## Responsibilities

- schedule events
- reschedule events
- cancel events
- manage participants
- manage reminders
- expose the family timeline

---

## Aggregate

- `Event`

---

## Owned Concepts

- Event
- Participant
- Reminder
- Schedule

---

## Invariants

- every event belongs to one family
- event schedule must be valid
- participants must be unique within an event
- reminders must reference a valid event schedule

---

## Events Emitted

- `EventScheduled`
- `EventRescheduled`
- `EventCancelled`
- `EventParticipantAdded`
- `EventParticipantRemoved`
- `ReminderAdded`
- `ReminderRemoved`

---

## Events Consumed

- `FamilyCreated`
- `MemberAdded`
- `MemberRemoved`
- `DependentAdded`
- `PetAdded`

---

## Related Feature Specs

- schedule-event
- reschedule-event
- cancel-event
- add-event-participant
- remove-event-participant
- add-reminder
- remove-reminder
- view-family-timeline