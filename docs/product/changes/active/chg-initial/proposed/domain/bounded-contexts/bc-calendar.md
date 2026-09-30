---
id: BC-CALENDAR
type: bounded-context
title: "Calendar"
status: draft
provenance:
  source: "openspec/specs/calendar/spec.md (Purpose); docs/_legacy/04_contexts/calendar.md (Purpose, Responsibilities, Boundaries With Other Contexts, Design Notes); docs/_legacy/00_product/surfaces/agenda.md (Data); src/backend/DomusMind.Domain/Calendar/**"
  confidence: "high"
  recovered-from: "documentation"
---

## Responsibility

Calendar is the temporal structure of household life. It answers what is happening in the household, when it happens and who is involved (observed: openspec/specs/calendar/spec.md, Purpose).

It owns:
- plans: time-bound commitments that affect one or more people in the household, with their schedule, participants, reminders and status (observed: calendar spec, Purpose; legacy calendar.md, Aggregate Roots);
- external calendar connections: a person's delegated, read-only link to a third-party calendar (Phase 1: Microsoft Outlook), with the provider calendars they selected, the sync horizon and the refresh settings (observed: calendar spec, Outlook Account Connection; legacy calendar.md, ExternalCalendarConnection);
- the imported external calendar entries held for projection into that person's Agenda (observed: legacy calendar.md, ExternalCalendarEntry).

Calendar is also the owner of the product's time vocabulary, including the Agenda, the unified temporal read surface. The Agenda is not a separate area: it reads from Calendar, Tasks and Lists without owning any of their records (observed: calendar spec, Purpose; legacy system-spec.md, Agenda is not a bounded context).

## Language

Household-facing language says **Plan**; the older domain documents and the code say **Event**. Both name the same thing (observed: calendar spec, Purpose, "User interfaces may present Events as Plans"; legacy calendar.md, Ubiquitous Language Notes). This baseline uses Plan.

The language is about time and coordination, not execution: schedule, start and end, participants, reminders offset before the start, cancellation. Around external calendars it speaks of connections, selected calendars (feeds), the sync horizon, synchronization and catch-up. Around the Agenda it speaks of scope (Household or one person), mode (Day, Week, Month), the board, the timeline and projected entries (observed: legacy calendar.md; agenda.md).

Legacy calendar.md asks to avoid ambiguous synonyms such as appointment, booking, calendar item or entry for plans (observed: legacy calendar.md, Ubiquitous Language Notes). "Entry" is reserved here for imported external calendar entries and Agenda entries.

## Boundaries

- Household identity and membership belong to Family; Calendar only references people as participants and never changes the household (observed: legacy calendar.md, Family Context).
- Area ownership belongs to Responsibilities; a plan may only reference an area for contextual grouping (observed: calendar spec, Event Scheduling; legacy calendar.md, Responsibility Context).
- Operational work belongs to Tasks. Calendar owns time-bound commitments; tasks and routines are Tasks' records even when they appear in the Agenda (observed: legacy calendar.md, Tasks Context).
- List items belong to Lists; the Agenda projects temporal list items but never edits them (observed: agenda.md, Projected List Items).
- Delivering reminder notifications is outside Calendar: Calendar defines when reminders are due, not how they are delivered (observed: calendar spec, Event Reminders).
- Imported external calendar data is read-only integration state and never becomes native household planning (observed: calendar spec, Purpose).
- Calendar must not absorb task completion, responsibility ownership, inventory, meal planning or administration (observed: legacy calendar.md, Design Notes).

## External Relationships

- **Family**: plan participants are household members; their existence in the household is checked when they are added (observed: calendar spec, Event Scheduling).
- **Responsibilities**: a plan may carry an optional area reference (observed: calendar spec; domain code keeps an optional area on the plan).
- **Tasks** and **Lists**: sources, together with Calendar, of the Agenda and the household timeline (observed: calendar spec, Household Timeline Projection). A plan may have a related list, shown in the Agenda as a compact cue (observed: agenda.md, Lists in Agenda).
- **Microsoft Outlook** (ACT-MICROSOFT-OUTLOOK): read through the person's delegated access; nothing is ever written back (observed: calendar spec, Outlook Account Connection; legacy system-spec.md).
- **Calendar Sync Scheduler** (ACT-CALENDAR-SYNC-SCHEDULER): refreshes stale connections in the background, by default every 60 minutes (observed: calendar spec, Background Feed Refresh).
